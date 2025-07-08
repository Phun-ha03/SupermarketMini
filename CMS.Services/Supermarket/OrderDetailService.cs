using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Authen.Genders;
using CMS.Models.Authen.Users;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.OrderDetails;
using CMS.Models.Supermarket.Orders;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.StockImportDetails;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly AICMSDBContext _context;

        public OrderDetailService(AICMSDBContext context
        )
        {
            _context = context;
        }

        public async Task<ApiResult<OrderDetailViewModel>> GetById(int id)
        {
            try
            {
                var OrderDetail = await _context.OrderDetails.AsNoTracking().FirstOrDefaultAsync(x => x.OrderID == id);
                if (OrderDetail == null)
                {
                    return new ApiErrorResult<OrderDetailViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<OrderDetailViewModel>(new OrderDetailViewModel(OrderDetail));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<OrderDetailViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Orders.AsNoTracking();

                var data = await query.Select(x => new OrderDetailViewModel())
                    .ToListAsync();
                data = data == null ? new List<OrderDetailViewModel>() : data;
                return new ApiSuccessResult<List<OrderDetailViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<OrderDetailViewModel>> Create(OrderDetailCreateRequest request)
        {
            try
            {
                var is_exists = (await _context.OrderDetails.Where(m => m.ProductID.Equals(request.ProductID))
                    .ToListAsync()).Any();
                if (is_exists)
                {
                    return new ApiErrorResult<OrderDetailViewModel>("Loại nguyên liệu đã tồn tại");
                }

                var newObject = new OrderDetail()
                {
                    //OrderDetailID = request.OrderDetailID,
                   // OrderID = request.OrderID,
                    ProductID = request.ProductID,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice,

                };

                _context.OrderDetails.Add(newObject);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<OrderDetailViewModel>(new OrderDetailViewModel(newObject));
                }
                return new ApiErrorResult<OrderDetailViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<OrderDetailViewModel>> Update(OrderDetailEditRequest request)
        {
            try
            {
                var is_exists = await _context.OrderDetails
                    .Where(m => m.OrderID.Equals(request.OrderID)
                        && m.ProductID != request.ProductID)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<OrderDetailViewModel>("Tên loại sản phẩm đã tồn tại");
                }

                var editObject = await _context.OrderDetails.FindAsync(request.OrderID);

                if (editObject == null)
                {
                    return new ApiErrorResult<OrderDetailViewModel>(ConstantHelper.UpdateNotfound);
                }

                editObject.ProductID = request.ProductID;
                editObject.OrderDetailID = request.OrderDetailID;
                editObject.Quantity = request.Quantity;
                editObject.UnitPrice = request.UnitPrice;
               

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<OrderDetailViewModel>(new OrderDetailViewModel(editObject));
                }
                else
                {
                    return new ApiErrorResult<OrderDetailViewModel>(ConstantHelper.UpdateError);
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
                var delObject = await _context.Categories.FindAsync(id);

                if (delObject == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                _context.Categories.Remove(delObject);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<int>(1);
                }
                else
                {
                    return new ApiErrorResult<int>(ConstantHelper.UpdateError);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }


        public async Task<ApiResult<PagedResult<OrderDetailViewModel>>> GetListPaging(GetOrderDetailPagingRequest request)
        {
            try
            {
                // 1. Tạo truy vấn IQueryable từ OrderDetails (AsNoTracking)
                var query = _context.OrderDetails
                    .Include(od => od.Product) // Bao gồm thông tin sản phẩm
                    .AsNoTracking();

                // 2. Lọc (nếu có)
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();

                    query = query.Where(od => od.Product.Name.ToLower().Contains(keyword)); // Ví dụ: lọc theo tên sản phẩm
                }

                // 3. Tính tổng số bản ghi
                int totalRow = await query.CountAsync();

                // 4. Phân trang và chuyển đổi sang OrderDetailViewModel
                var data = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(od => new OrderDetailViewModel
                    {
                        OrderDetailID = od.OrderDetailID,
                        OrderID = od.OrderID,
                        ProductID = od.ProductID,
                        Quantity = od.Quantity,
                        UnitPrice = od.UnitPrice,
                       
                    })
                    .ToListAsync();

                // 5. Tạo PagedResult
                var pageResult = new PagedResult<OrderDetailViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<OrderDetailViewModel>() : data
                };

                // 6. Trả về ApiSuccessResult
                return new ApiSuccessResult<PagedResult<OrderDetailViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi (sử dụng LogHelper của bạn)
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<List<OrderDetailViewModel>> GetDetailsByOrderId(int orderId)
        {
            var details = await _context.OrderDetails
                .Where(d => d.OrderID == orderId)
                .Include(d => d.Product)
                .Include(d => d.ProductUnit) // Đảm bảo đơn vị được load
                .ToListAsync();

            return details.Select(d => new OrderDetailViewModel
            {
                OrderDetailID = d.OrderDetailID,
                OrderID = d.OrderID,
                ProductID = d.ProductID,
                Name = d.Product?.Name,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                UnitID = d.UnitID,
                UnitName = d.ProductUnit?.UnitName
            }).ToList();
        }

    }
}
