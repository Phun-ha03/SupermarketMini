using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.StockImports;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket
{
    public class StockImportService : IStockImportService
    {
        private readonly AICMSDBContext _context;
        private readonly IStockImportDetailService _stockImportDetailService;
        private readonly IProductService _productService;

        public StockImportService(AICMSDBContext context, IStockImportDetailService stockImportDetailService, IProductService productService)
        {
            _context = context;
            _stockImportDetailService = stockImportDetailService;
            _productService = productService;
        }

        // Get Stock Import by ID with details
        public async Task<ApiResult<StockImportViewModel>> GetById(int id)
        {
            try
            {
                var stockImport = await _context.StockImports
                    .Include(si => si.Supplier)
                    .Include(si => si.StockImportDetails)
                        .ThenInclude(sid => sid.Product)
                    .FirstOrDefaultAsync(si => si.StockImportID == id);

                if (stockImport == null)
                {
                    return new ApiErrorResult<StockImportViewModel>(ConstantHelper.DataNotfound);
                }

                return new ApiSuccessResult<StockImportViewModel>(new StockImportViewModel(stockImport));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), nameof(GetById));
                throw;
            }
        }

        // Get all Stock Imports
        public async Task<ApiResult<List<StockImportViewModel>>> GetAll()
        {
            try
            {
                var data = await _context.StockImports.AsNoTracking()
                    .Select(x => new StockImportViewModel(x))
                    .ToListAsync() ?? new List<StockImportViewModel>();

                return new ApiSuccessResult<List<StockImportViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), nameof(GetAll));
                throw;
            }
        }

        // Create new Stock Import
        public async Task<ApiResult<StockImportViewModel>> Create(StockImportCreateRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (request == null || request.StockImportDetails == null || !request.StockImportDetails.Any())
                {
                    return new ApiErrorResult<StockImportViewModel>("Dữ liệu không hợp lệ hoặc không có chi tiết nhập kho.");
                }

                var supplierExists = await _context.Suppliers.AnyAsync(s => s.SupplierID == request.SupplierID);
                if (!supplierExists)
                {
                    return new ApiErrorResult<StockImportViewModel>("Nhà cung cấp không tồn tại.");
                }

                var newStockImport = new StockImport
                {
                    SupplierID = request.SupplierID,
                    ImportDate = (DateTime)request.ImportDate,
                    DiscountAmount = request.DiscountAmount,
                    TotalCost = 0,
                    StockImportDetails = new List<StockImportDetail>()
                };

                _context.StockImports.Add(newStockImport);
                await _context.SaveChangesAsync(); // lấy ImportID

                decimal totalCostBeforeDiscount = 0;

                foreach (var detailRequest in request.StockImportDetails)
                {
                    var product = await _context.Products
                 .Include(p => p.ProductUnits)
                 .FirstOrDefaultAsync(p => p.ProductID == detailRequest.ProductID);
                    if (product == null)
                    {
                        await transaction.RollbackAsync();
                        return new ApiErrorResult<StockImportViewModel>($"Không tìm thấy sản phẩm với ID: {detailRequest.ProductID}");
                    }
                    var unit = await _context.ProductUnits
               .FirstOrDefaultAsync(u => u.UnitID == detailRequest.UnitID);

                    if (unit == null || unit.ProductID != product.ProductID)
                    {
                        await transaction.RollbackAsync();
                        return new ApiErrorResult<StockImportViewModel>($"Đơn vị không hợp lệ cho sản phẩm ID: {detailRequest.ProductID}");
                    }

                    var newDetail = new StockImportDetail
                    {
                        StockImportID = newStockImport.StockImportID,
                        ProductID = detailRequest.ProductID,
                        Quantity = detailRequest.Quantity,
                        CostPrice = detailRequest.CostPrice,
                        ExpirationDate = detailRequest.ExpirationDate,
                        UnitID = (int)detailRequest.UnitID
                    };

                    newStockImport.StockImportDetails.Add(newDetail);

                    // Update tồn kho
                    int baseQuantity = (int)(detailRequest.Quantity * unit.ConversionRate);
                    product.StockQuantity += baseQuantity;

                    _context.Products.Update(product);

                    totalCostBeforeDiscount += detailRequest.Quantity * detailRequest.CostPrice;
                }

                // Tính tổng tiền cuối cùng
                var finalTotalCost = totalCostBeforeDiscount - (request.DiscountAmount ?? 0);
                if (finalTotalCost < 0) finalTotalCost = 0;

                newStockImport.TotalCost = finalTotalCost;
                _context.StockImports.Update(newStockImport);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiSuccessResult<StockImportViewModel>(new StockImportViewModel(newStockImport));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LogHelper.writeLog(ex.ToString(), nameof(Create));
                throw;
            }
        }

        // Delete a Stock Import
        public async Task<ApiResult<int>> Delete(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var stockImport = await _context.StockImports
                    .Include(si => si.StockImportDetails)
                    .FirstOrDefaultAsync(si => si.StockImportID == id);

                if (stockImport == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                // Kiểm tra xem có chi tiết nhập kho không.  Nếu không có, không cần lặp.
                if (stockImport.StockImportDetails != null && stockImport.StockImportDetails.Any())
                {
                    // Trừ lại số lượng tồn kho
                    foreach (var detail in stockImport.StockImportDetails)
                    {
                        var product = await _context.Products.Include(p => p.ProductUnits).FirstOrDefaultAsync(p => p.ProductID == detail.ProductID);
                        if (product != null)
                        {
                            
                            var unit = product.ProductUnits.FirstOrDefault(u => u.UnitID == detail.UnitID); 
                            if (unit != null)
                            {
                                int baseQuantity = (int)(detail.Quantity * unit.ConversionRate);
                                product.StockQuantity -= baseQuantity;
                                _context.Products.Update(product);
                            }
                            else
                            {
                                LogHelper.writeLog($"Không tìm thấy đơn vị với UnitID: {detail.UnitID} cho sản phẩm ID: {detail.ProductID} khi xóa StockImport", nameof(Delete));
                            }
                        }
                        else
                        {
                            LogHelper.writeLog($"Không tìm thấy sản phẩm với ID: {detail.ProductID} khi xóa StockImport", nameof(Delete));
                        }
                    }
                    // Xóa các StockImportDetails trước khi xóa StockImport
                    _context.StockImportDetails.RemoveRange(stockImport.StockImportDetails);
                }

                _context.StockImports.Remove(stockImport);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    await transaction.CommitAsync();
                    return new ApiSuccessResult<int>(1);
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new ApiErrorResult<int>(ConstantHelper.UpdateError); 
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LogHelper.writeLog(ex.ToString(), nameof(Delete));
                throw;
            }
        }


        // Get Stock Imports with Paging
        public async Task<ApiResult<PagedResult<StockImportViewModel>>> GetListPaging(GetStockImportPagingRequest request)
        {
            try
            {
                var query = from p in _context.StockImports.AsNoTracking()
                    join s in _context.Suppliers on p.SupplierID equals s.SupplierID
                            select new StockImportViewModel
                            {
                                StockImportID=p.StockImportID,
                                SupplierID=p.SupplierID,
                                TotalCost=p.TotalCost,
                                ImportDate=p.ImportDate,
                                SupplierName=s.SupplierName,
                               // StockImportDetails=p.StockImportDetails,
                            };

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();
                    query = query.Where(x => x.SupplierID.ToString().Contains(keyword));
                }

                int totalRow = await query.CountAsync();

                var data = await query.OrderByDescending(p => p.StockImportID)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync();

                var pageResult = new PagedResult<StockImportViewModel>
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data ?? new List<StockImportViewModel>()
                };

                return new ApiSuccessResult<PagedResult<StockImportViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
    }
}
