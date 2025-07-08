using ClosedXML.Excel;
using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Categories;
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
    public class CategoryService : ICategoryService
    {
        private readonly AICMSDBContext _context;

        public CategoryService(AICMSDBContext context
        )
        {
            _context = context;
        }

        public async Task<ApiResult<CategoryViewModel>> GetById(int id)
        {
            try
            {
                var function = await _context.Categories.FindAsync(id);

                if (function == null)
                {
                    return new ApiErrorResult<CategoryViewModel>(ConstantHelper.DataNotfound);
                }

                return new ApiSuccessResult<CategoryViewModel>(new CategoryViewModel(function));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<CategoryViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Categories.AsNoTracking().AsQueryable();

                var data = await query.Select(x => new CategoryViewModel(x))
                    .ToListAsync();

                data = data == null ? new List<CategoryViewModel>() : data;

                return new ApiSuccessResult<List<CategoryViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<CategoryViewModel>> Create(CategoryCreateRequest request)
        {
            try
            {
                var is_exists = (await _context.Categories.Where(m => m.CategoryName.Equals(request.CategoryName))
                    .ToListAsync()).Any();
                if (is_exists)
                {
                    return new ApiErrorResult<CategoryViewModel>("Loại nguyên liệu đã tồn tại");
                }

                var newObject = new Category()
                {
                    CategoryID = request.CategotyID,
                    CategoryName = request.CategoryName,
                    Description= request.Description,
                    CreateAt= request.CreateAt,
                    UpdateAt= request.UpdateAt,

                };

                _context.Categories.Add(newObject);

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<CategoryViewModel>(new CategoryViewModel(newObject));
                }
                return new ApiErrorResult<CategoryViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<CategoryViewModel>> Update(CategoryEditRequest request)
        {
            try
            {
                var is_exists = await _context.Categories
                    .Where(m => m.CategoryName.Equals(request.CategoryName)
                        && m.CategoryID != request.CategoryID)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<CategoryViewModel>("Tên loại sản phẩm đã tồn tại");
                }

                var editObject = await _context.Categories.FindAsync(request.CategoryID);

                if (editObject == null)
                {
                    return new ApiErrorResult<CategoryViewModel>(ConstantHelper.UpdateNotfound);
                }

                editObject.CategoryName = request.CategoryName;
                editObject.CategoryID = request.CategoryID;
                editObject.Description = request.Description;
                editObject.CreateAt = request.CreateAt;
                editObject.UpdateAt = request.UpdateAt;

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<CategoryViewModel>(new CategoryViewModel(editObject));
                }
                else
                {
                    return new ApiErrorResult<CategoryViewModel>(ConstantHelper.UpdateError);
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

        public async Task<ApiResult<PagedResult<CategoryViewModel>>> GetListPaging(GetCategoryPagingRequest request)
        {
            try
            {
                var query = _context.Categories.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();

                    query = query.Where(x => x.CategoryName.ToLower().Contains(keyword)
                                || x.Description.ToLower().Contains(keyword));
                }

                int totalRow = await query.CountAsync();

                var data = await query.OrderByDescending(p => p.CategoryID)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new CategoryViewModel(x))
                    .ToListAsync();

                var pageResult = new PagedResult<CategoryViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<CategoryViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<CategoryViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<byte[]> ExportCategoriesToExcelAsync()
        {
            try
            {
                var categories = await _context.Categories
                    .AsNoTracking()
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Categories");

                // Header
                worksheet.Cell(1, 1).Value = "Tên danh mục";
                worksheet.Cell(1, 2).Value = "Mô tả";

                // Nội dung
                int row = 2;
                foreach (var s in categories)
                {
                    worksheet.Cell(row, 1).Value = s.CategoryName;
                    worksheet.Cell(row, 2).Value = s.Description;
                    row++;
                }

                // Auto-fit
                worksheet.Columns().AdjustToContents();

                // Xuất file ra byte[]
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

    }
}
