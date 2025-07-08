using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.StockImportDetails;
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
    public class StockImportDetailService : IStockImportDetailService
    {
        private readonly AICMSDBContext _context;

        public StockImportDetailService(AICMSDBContext context
        )
        {
            _context = context;
        }

        public async Task<ApiResult<StockImportDetailViewModel>> GetById(int id)
        {
            try
            {
                var function = await _context.StockImportDetails.FindAsync(id);

                if (function == null)
                {
                    return new ApiErrorResult<StockImportDetailViewModel>(ConstantHelper.DataNotfound);
                }

                return new ApiSuccessResult<StockImportDetailViewModel>(new StockImportDetailViewModel(function));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<StockImportDetailViewModel>>> GetAll()
        {
            try
            {
                var query = _context.StockImportDetails.AsNoTracking().AsQueryable();

                var data = await query.Select(x => new StockImportDetailViewModel(x))
                    .ToListAsync();

                data = data == null ? new List<StockImportDetailViewModel>() : data;

                return new ApiSuccessResult<List<StockImportDetailViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<StockImportDetailViewModel>> Create(StockImportDetailCreateRequest request)
        {
            try
            {
                var is_exists = (await _context.StockImportDetails.Where(m => m.ProductID.Equals(request.ProductID))
                    .ToListAsync()).Any();
                if (is_exists)
                {
                    return new ApiErrorResult<StockImportDetailViewModel>("Loại nguyên liệu đã tồn tại");
                }

                var newObject = new StockImportDetail()
                {
                  //  ImportDetailID = request.ImportID,
                   // ImportID = request.ImportID,
                    ProductID = request.ProductID,
                    CostPrice = request.CostPrice,
                    Quantity = request.Quantity,
                    ExpirationDate = request.ExpirationDate,


                };

                _context.StockImportDetails.Add(newObject);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<StockImportDetailViewModel>(new StockImportDetailViewModel(newObject));
                }
                return new ApiErrorResult<StockImportDetailViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<StockImportDetailViewModel>> Update(StockImportDetailEditRequest request)
        {
            try
            {
                var is_exists = await _context.StockImportDetails
                    .Where(m => m.StockImportID.Equals(request.StockImportID)
                        && m.ImportDetailID != request.ImportDetailID)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<StockImportDetailViewModel>("Tên loại sản phẩm đã tồn tại");
                }

                var editObject = await _context.StockImportDetails.FindAsync(request.ImportDetailID);

                if (editObject == null)
                {
                    return new ApiErrorResult<StockImportDetailViewModel>(ConstantHelper.UpdateNotfound);
                }

                editObject.StockImportID = request.StockImportID;
                editObject.ImportDetailID = request.ImportDetailID;
                editObject.Quantity = request.Quantity;
                editObject.CostPrice = request.CostPrice;
                editObject.ExpirationDate = request.ExpirationDate;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<StockImportDetailViewModel>(new StockImportDetailViewModel(editObject));
                }
                else
                {
                    return new ApiErrorResult<StockImportDetailViewModel>(ConstantHelper.UpdateError);
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
                var delObject = await _context.StockImportDetails.FindAsync(id);

                if (delObject == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                _context.StockImportDetails.Remove(delObject);

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

        public async Task<ApiResult<PagedResult<StockImportDetailViewModel>>> GetListPaging(GetStockImportDetailPagingRequest request)
        {
            try
            {
                var query = _context.StockImportDetails
                    .Include(x => x.Product) // load thêm tên sản phẩm
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();

                    query = query.Where(x =>
                        x.Product.Name.ToLower().Contains(keyword) ||
                        x.ImportDetailID.ToString().Contains(keyword)
                    );
                }

                int totalRow = await query.CountAsync();

                var data = await query
                    .OrderByDescending(x => x.ImportDetailID)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new StockImportDetailViewModel
                    {
                        ImportDetailID = x.ImportDetailID,
                        Name = x.Product.Name
                    })
                    .ToListAsync();

                var pageResult = new PagedResult<StockImportDetailViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data ?? new List<StockImportDetailViewModel>()
                };

                return new ApiSuccessResult<PagedResult<StockImportDetailViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<List<StockImportDetailViewModel>> GetDetailsByImportId(int importId)
        {
            var details = await _context.StockImportDetails
                .Where(d => d.StockImportID == importId)
                .Include(d => d.Product) // nếu muốn lấy tên sản phẩm
                .ToListAsync();

            return details.Select(d => new StockImportDetailViewModel
            {
                ProductID = d.ProductID,
                Name = d.Product?.Name,
                Quantity = d.Quantity,
                CostPrice = d.CostPrice,
                ExpirationDate = d.ExpirationDate
            }).ToList();
        }


    }

}
