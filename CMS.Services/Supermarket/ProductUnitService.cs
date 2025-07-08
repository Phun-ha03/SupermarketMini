using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.ProductUnits;
using CMS.Models.Supermarket.ProductUnits;
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
    public class ProductUnitService : IProductUnitService
    {
        private readonly AICMSDBContext _context;

        public ProductUnitService(AICMSDBContext context
        )
        {
            _context = context;
        }

        public async Task<ApiResult<ProductUnitViewModel>> GetById(int id)
        {
            try
            {
                var function = await _context.ProductUnits
               .Include(x => x.Product)
               .FirstOrDefaultAsync(x => x.UnitID == id);


                if (function == null)
                {
                    return new ApiErrorResult<ProductUnitViewModel>(ConstantHelper.DataNotfound);
                }

                return new ApiSuccessResult<ProductUnitViewModel>(new ProductUnitViewModel(function));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<ProductUnitViewModel>>> GetAll()
        {
            try
            {
                var query = _context.ProductUnits.AsNoTracking().AsQueryable();

                var data = await _context.ProductUnits
                .AsNoTracking()
                 .Include(x => x.Product)
               .Select(x => new ProductUnitViewModel(x))
               .ToListAsync();


                data = data == null ? new List<ProductUnitViewModel>() : data;

                return new ApiSuccessResult<List<ProductUnitViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<ProductUnitViewModel>> Create(ProductUnitCreateRequest request)
        {
            try
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductID == request.ProductID);

                if (product == null)
                {
                    return new ApiErrorResult<ProductUnitViewModel>("Không tìm thấy sản phẩm với mã này.");
                }
                if (request.IsBaseUnit)
                {
                    var hasBaseUnit = await _context.ProductUnits
                        .AnyAsync(u => u.ProductID == request.ProductID && u.IsBaseUnit);
                    if (hasBaseUnit)
                    {
                        return new ApiErrorResult<ProductUnitViewModel>("Sản phẩm đã có đơn vị cơ sở.");
                    }
                }
                var newObject = new ProductUnit
                {
                    ProductID = product.ProductID,
                    UnitName = request.UnitName,
                    UnitPrice = request.UnitPrice,
                    ConversionRate = request.ConversionRate,
                    IsBaseUnit = request.IsBaseUnit,
                  
                };

                _context.ProductUnits.Add(newObject);
                // Nếu là đơn vị cơ sở → cập nhật Product.Price
                if (request.IsBaseUnit)
                {
                    product.Price = (decimal)request.UnitPrice;
                    _context.Products.Update(product);
                }
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<ProductUnitViewModel>(new ProductUnitViewModel(newObject));
                }

                return new ApiErrorResult<ProductUnitViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<ProductUnitViewModel>> Update(ProductUnitEditRequest request)
        {
            try
            {
                // Kiểm tra đơn vị có tồn tại không
                var unit = await _context.ProductUnits
                    .Include(u => u.Product)
                    .FirstOrDefaultAsync(u => u.UnitID == request.UnitID);

                if (unit == null)
                {
                    return new ApiErrorResult<ProductUnitViewModel>("Không tìm thấy đơn vị cần cập nhật.");
                }

                // Kiểm tra trùng tên đơn vị khác ID
                var isExists = await _context.ProductUnits
                    .AnyAsync(u => u.UnitName == request.UnitName && u.UnitID != request.UnitID && u.ProductID == unit.ProductID);
                if (isExists)
                {
                    return new ApiErrorResult<ProductUnitViewModel>("Tên đơn vị đã tồn tại cho sản phẩm này.");
                }

                // Nếu chuyển thành đơn vị cơ sở → kiểm tra xem đã có đơn vị cơ sở khác chưa
                if (request.IsBaseUnit && !unit.IsBaseUnit)
                {
                    var hasBaseUnit = await _context.ProductUnits
                        .AnyAsync(u => u.ProductID == unit.ProductID && u.IsBaseUnit && u.UnitID != request.UnitID);
                    if (hasBaseUnit)
                    {
                        return new ApiErrorResult<ProductUnitViewModel>("Sản phẩm đã có đơn vị cơ sở khác.");
                    }

                    // Cập nhật giá sản phẩm nếu là đơn vị cơ sở mới
                    if (unit.Product != null)
                    {
                        unit.Product.Price = (decimal)request.UnitPrice;
                        _context.Products.Update(unit.Product);
                    }
                }

                // Cập nhật dữ liệu
                unit.UnitName = request.UnitName;
                unit.UnitPrice = request.UnitPrice;
                unit.ConversionRate = request.ConversionRate;
                unit.IsBaseUnit = request.IsBaseUnit;

                _context.ProductUnits.Update(unit);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<ProductUnitViewModel>(new ProductUnitViewModel(unit));
                }

                return new ApiErrorResult<ProductUnitViewModel>(ConstantHelper.UpdateError);
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
                var delObject = await _context.ProductUnits.FindAsync(id);

                if (delObject == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                _context.ProductUnits.Remove(delObject);

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

        public async Task<ApiResult<PagedResult<ProductUnitViewModel>>> GetListPaging(GetProductUnitPagingRequest request)
        {
            try
            {
                var query = from p in _context.ProductUnits.AsNoTracking()
                            join prod in _context.Products on p.ProductID equals prod.ProductID
                            where p.ProductID == prod.ProductID
                            select new ProductUnitViewModel
                            {
                                UnitID = p.UnitID,
                                Name = prod.Name,
                                UnitName = p.UnitName,
                                ConversionRate = p.ConversionRate,
                                UnitPrice = p.UnitPrice,
                                IsBaseUnit = p.IsBaseUnit,
                                ProductID = p.ProductID,
                                ProductCode = prod.ProductCode
                            };
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();
                    query = query.Where(x => x.Name.ToLower().Contains(keyword));
                }

                int totalRow = await query.CountAsync();

                var data = await query.OrderByDescending(p => p.UnitID)
               .Skip((request.PageIndex - 1) * request.PageSize)
               .Take(request.PageSize)
               //.Select(x => new ProductUnitViewModel
               //{
               //    UnitID = x.UnitID,
               //    ProductID = x.ProductID,
               //    UnitName = x.UnitName,
               //    ConversionRate = x.ConversionRate,
               //    UnitPrice = x.UnitPrice,
               //    IsBaseUnit = x.IsBaseUnit,
               //    Name = x.Product != null ? x.Product.Name : "",
               //    ProductCode = x.Product != null ? x.Product.ProductCode : ""
               //})
               .ToListAsync();

                var pageResult = new PagedResult<ProductUnitViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<ProductUnitViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<ProductUnitViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

    }
}
