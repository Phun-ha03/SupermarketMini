using ClosedXML.Excel;
using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Authen.Users;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Orders;
using CMS.Models.Supermarket.Reports;
using CMS.Models.Supermarket.StockImports;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket
{
    public class OrderService : IOrderService
    {
        private readonly AICMSDBContext _context;
        public OrderService(AICMSDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<OrderViewModel>> GetById(int id)
        {
            try
            {
                var order = await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.ProductUnit)
                        .Include(o => o.Customer)
                    .FirstOrDefaultAsync(x => x.OrderID == id);

                if (order == null)
                {
                    return new ApiErrorResult<OrderViewModel>(ConstantHelper.DataNotfound);
                }
                var viewModel = new OrderViewModel(order);

                return new ApiSuccessResult<OrderViewModel>(new OrderViewModel(order));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), nameof(GetById));
                throw;
            }
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Orders.AsNoTracking().Include(o => o.OrderDetails);
                var data = await query.Select(x => new OrderViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<OrderViewModel>() : data;
                return new ApiSuccessResult<List<OrderViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<OrderViewModel>>> GetAllActive()
        {
            try
            {
                var query = _context.Orders.AsNoTracking().Where(m => m.CustomerID == 1);

                var data = await query.Select(x => new OrderViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<OrderViewModel>() : data;
                return new ApiSuccessResult<List<OrderViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<PagedResult<OrderViewModel>>> GetListPaging(GetOrderPagingRequest request)
        {
            try
            {
                var query = from p in _context.Orders.AsNoTracking()
                            join s in _context.Customers on p.CustomerID equals s.CustomerID into joined
                            from customer in joined.DefaultIfEmpty()
                            select new OrderViewModel
                            {
                                OrderID = p.OrderID,
                                CustomerID = p.CustomerID,
                                Name = p.CustomerID == null || p.CustomerID == 0 ? "Khách lẻ" : customer.Name,
                                Discount = p.Discount,
                                PaymenMethod = p.PaymenMethod,
                                TotalAmount = p.TotalAmount,
                                CreateAt = p.CreateAt,
                            };

                // Lọc theo ngày
                if (request.SearchDate.HasValue)
                {
                    var searchDate = request.SearchDate.Value.Date;
                    query = query.Where(x => x.CreateAt.Date == searchDate);
                }

                // Lọc thêm theo keyword nếu có
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x =>
                        x.PaymenMethod.Contains(request.Keyword) ||
                        x.OrderID.ToString().Contains(request.Keyword)
                    );
                }

                int totalRow = await query.CountAsync();
                var data = await query
                    .OrderByDescending(x => x.OrderID)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var pageResult = new PagedResult<OrderViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };

                return new ApiSuccessResult<PagedResult<OrderViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<OrderViewModel>> Create(OrderCreateRequest request, int currentUserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (request == null || request.OrderDetails == null || !request.OrderDetails.Any())
                {
                    return new ApiErrorResult<OrderViewModel>("Dữ liệu không hợp lệ hoặc không có chi tiết đơn hàng.");
                }

                var newOrder = new Order
                {
                    CustomerID = (request.CustomerID.HasValue && request.CustomerID > 0) ? request.CustomerID : null,
                    Discount = request.Discount,
                    PaymenMethod = request.PaymentMethod,
                    CreateAt = DateTime.Now,
                    CreatedBy = currentUserId, // Gán ID tài khoản đang đăng nhập
                    OrderDetails = new List<OrderDetail>()
                };

                decimal orderSubtotal = 0;

                foreach (var item in request.OrderDetails)
                {
                    var product = await _context.Products.FindAsync(item.ProductID);
                    var unit = await _context.ProductUnits
                        .FirstOrDefaultAsync(u => u.UnitID == item.UnitID && u.ProductID == item.ProductID);

                    if (product == null || unit == null)
                    {
                        await transaction.RollbackAsync();
                        return new ApiErrorResult<OrderViewModel>($"Không tìm thấy sản phẩm hoặc đơn vị tính hợp lệ cho sản phẩm ID {item.ProductID}");
                    }

                    var orderDetail = new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitID = item.UnitID,
                        UnitPrice = item.UnitPrice
                    };

                    newOrder.OrderDetails.Add(orderDetail);
                    orderSubtotal += item.UnitPrice * item.Quantity;

                    var baseQuantity = item.Quantity * unit.ConversionRate;

                    var batches = await _context.StockImportDetails
                        .Where(x => x.ProductID == item.ProductID
                                && x.Quantity > x.UsedQuantity
                                && (x.ExpirationDate == null || x.ExpirationDate >= DateTime.Today))
                        .OrderBy(x => x.ExpirationDate)
                        .ToListAsync();

                    foreach (var batch in batches)
                    {
                        if (baseQuantity <= 0) break;

                        decimal available = batch.Quantity - batch.UsedQuantity;
                        if (available <= 0) continue;

                        if (available >= baseQuantity)
                        {
                            batch.UsedQuantity += baseQuantity;
                            baseQuantity = 0;
                        }
                        else
                        {
                            batch.UsedQuantity = batch.Quantity;
                            baseQuantity -= available;
                        }

                        _context.StockImportDetails.Update(batch);
                    }

                    product.StockQuantity -= item.Quantity * unit.ConversionRate;
                    if (product.StockQuantity < 0) product.StockQuantity = 0;
                    _context.Products.Update(product);
                }

                int usedPoints = request.UsedPoints ?? 0;
                decimal loyaltyDiscount = 0;

                if (request.CustomerID.HasValue && request.CustomerID > 0 && usedPoints > 0)
                {
                    var customer = await _context.Customers.FindAsync(request.CustomerID.Value);
                    if (customer == null || customer.LoyalPoints < usedPoints)
                    {
                        await transaction.RollbackAsync();
                        return new ApiErrorResult<OrderViewModel>("Khách hàng không đủ điểm để sử dụng.");
                    }

                    customer.LoyalPoints -= usedPoints;
                    _context.Customers.Update(customer);
                }

                decimal discountAmount = request.Discount;
                decimal finalTotal = orderSubtotal - discountAmount - loyaltyDiscount;
                newOrder.TotalAmount = Math.Max(finalTotal, 0);

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync();

                if (request.CustomerID.HasValue && request.CustomerID > 0 && newOrder.TotalAmount >= 100000)
                {
                    var customer = await _context.Customers.FindAsync(request.CustomerID.Value);
                    if (customer != null)
                    {
                        int earnedPoints = (int)(newOrder.TotalAmount / 100000);
                        customer.LoyalPoints += earnedPoints;
                        _context.Customers.Update(customer);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiSuccessResult<OrderViewModel>(new OrderViewModel(newOrder));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LogHelper.writeLog(ex.ToString(), nameof(Create));
                throw;
            }
        }


        public async Task<ApiResult<OrderViewModel>> Update(OrderEditRequest request)
        {
            try
            {
                var is_exists = await _context.Orders
                    .Where(m => m.OrderID.Equals(request.OrderID)
                        && m.OrderID != request.OrderID)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<OrderViewModel>("Tên loại sản phẩm đã tồn tại");
                }

                var editObject = await _context.Orders.FindAsync(request.OrderID);

                if (editObject == null)
                {
                    return new ApiErrorResult<OrderViewModel>(ConstantHelper.UpdateNotfound);
                }

               
                editObject.OrderID = request.OrderID;
                editObject.CustomerID = request.CustomerID;
                editObject.Discount = request.Discount;
                editObject.CreateAt = request.CreateAt;
                editObject.PaymenMethod = request.PaymenMethod;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<OrderViewModel>(new OrderViewModel(editObject));
                }
                else
                {
                    return new ApiErrorResult<OrderViewModel>(ConstantHelper.UpdateError);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<int>> Delete(int id)
        {
            try
            {
                // Tìm đơn hàng cần xóa và bao gồm (Include) các OrderDetails liên quan
                var orderToDelete = await _context.Orders
                                                 .Include(o => o.OrderDetails)
                                                 .FirstOrDefaultAsync(o => o.OrderID == id);

                if (orderToDelete == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                // Xóa tất cả các OrderDetails liên quan đến đơn hàng này
                if (orderToDelete.OrderDetails != null && orderToDelete.OrderDetails.Any())
                {
                    _context.OrderDetails.RemoveRange(orderToDelete.OrderDetails);
                }

                // Sau khi đã xóa các OrderDetails liên quan, tiến hành xóa đơn hàng
                _context.Orders.Remove(orderToDelete);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<int>(result);
                }
                else
                {
                    return new ApiErrorResult<int>(ConstantHelper.UpdateError); // Có thể nên đổi ConstantHelper.DeleteError
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public List<RevenueByDateDto> GetRevenueByDateRange(DateTime fromDate, DateTime toDate)
        {
            var result = _context.Orders
                .Where(o => o.CreateAt >= fromDate && o.CreateAt <= toDate)
                .GroupBy(o => o.CreateAt.Date)
                .Select(g => new RevenueByDateDto
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(x => x.TotalAmount - x.Discount),
                    TotalOrders = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToList();

            return result;
        }
        public async Task<byte[]> ExportOrdersToExcelAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.CreateAt.Date >= fromDate.Date && o.CreateAt.Date <= toDate.Date)
                    .AsNoTracking()
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Orders");

                // Header
                worksheet.Cell(1, 1).Value = "Mã hóa đơn";
                worksheet.Cell(1, 2).Value = "Khách hàng";
                worksheet.Cell(1, 3).Value = "Ngày tạo";
                worksheet.Cell(1, 4).Value = "Phương thức thanh toán";
                worksheet.Cell(1, 5).Value = "Tổng tiền";

                // Nội dung
                int row = 2;
                foreach (var order in orders)
                {
                    worksheet.Cell(row, 1).Value = order.OrderID;
                    worksheet.Cell(row, 2).Value = order.Customer?.Name ?? "Khách lẻ";
                    worksheet.Cell(row, 3).Value = order.CreateAt.ToString("dd/MM/yyyy");
                    worksheet.Cell(row, 4).Value = order.PaymenMethod;
                    worksheet.Cell(row, 5).Value = order.TotalAmount;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

    }
}
