using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.Genders;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen
{
    public class GenderService: IGenderService
    {
        private readonly AICMSDBContext _context;
        public GenderService(AICMSDBContext context)
        { 
            _context = context;
        }
        public async Task<ApiResult<GenderViewModel>> GetById(int id)
        {
            try
            {
                var gender = await _context.Genders.AsNoTracking().FirstOrDefaultAsync(x => x.GenderId == id);
                if (gender == null)
                {
                    return new ApiErrorResult<GenderViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<GenderViewModel>(new GenderViewModel(gender));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<GenderViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Genders.AsNoTracking();

                var data = await query.Select(x => new GenderViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<GenderViewModel>() : data;
                return new ApiSuccessResult<List<GenderViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<PagedResult<GenderViewModel>>> GetListPaging(GetGenderPagingRequest request)
        {
            try
            {
                var query = _context.Genders.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.GenderName.Contains(request.Keyword) || x.GenderDesc.Contains(request.Keyword));
                }

                /*var query = from p in _context.Products
                            join pt in _context.ProductTranslations
                            on p.Id equals pt.ProductId
                            join pc in _context.ProductCategories
                            on p.Id equals pc.ProductId
                            join c in _context.Categories
                            on pc.CategoriId equals c.Id
                            select new { p, pt };
                if (!string.IsNullOrEmpty(request.Keyword)) {
                    query = query.Where(x => x.pt.Name.Contains(request.Keyword));
                }*/
                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new GenderViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<GenderViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<GenderViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<GenderViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<GenderViewModel>> Create(GenderCreateRequest request)
        {
            try
            {
                var is_exists = await _context.Genders
                    .Where(m => m.GenderName.Equals(request.GenderName))
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<GenderViewModel>(ConstantHelper.GenderExists);
                }
                var gender = new Gender()
                {
                    GenderName = request.GenderName,
                    GenderDesc = request.GenderDesc
                };
                _context.Genders.Add(gender);
                await _context.SaveChangesAsync();
                if (gender.GenderId <= 0)
                {
                    return new ApiErrorResult<GenderViewModel>(ConstantHelper.UpdateError);
                }
                return new ApiSuccessResult<GenderViewModel>(new GenderViewModel(gender));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<GenderViewModel>> Update(GenderEditRequest request)
        {
            try
            {
                var is_exists = await _context.Genders
                    .Where(m => m.GenderName.Equals(request.GenderName)
                        && m.GenderId != request.GenderId)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<GenderViewModel>(ConstantHelper.GenderExists);
                }
                var gender = await _context.Genders.FindAsync(request.GenderId);
                if (gender == null)
                {
                    return new ApiErrorResult<GenderViewModel>(ConstantHelper.UpdateNotfound);
                }
                gender.GenderName = request.GenderName;
                gender.GenderDesc = request.GenderDesc;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<GenderViewModel>(new GenderViewModel(gender));
                }
                else
                {
                    return new ApiErrorResult<GenderViewModel>(ConstantHelper.UpdateError);
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
                var gender = await _context.Genders.FindAsync(id);
                if (gender == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }
                _context.Genders.Remove(gender);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<int>(result);
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
    }
}
