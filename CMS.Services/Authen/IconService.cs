using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.Icons;
using CMS.Models.Authen.Users;
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
    public class IconService:IIconService
    {
        private readonly AICMSDBContext _context;
        public IconService(AICMSDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<IconViewModel>> GetById(int id)
        {
            try
            {
                var icon = await _context.Icons.AsNoTracking().FirstOrDefaultAsync(x => x.IconId == id);
                if (icon == null)
                {
                    return new ApiErrorResult<IconViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<IconViewModel>(new IconViewModel(icon));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<List<IconViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Icons.AsNoTracking();

                var data = await query.Select(x => new IconViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<IconViewModel>() : data;
                return new ApiSuccessResult<List<IconViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<IconViewModel>>> GetAllActive()
        {
            try
            {
                var query = _context.Icons.AsNoTracking().Where(m => m.StatusId == 1);

                var data = await query.Select(x => new IconViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<IconViewModel>() : data;
                return new ApiSuccessResult<List<IconViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<PagedResult<IconViewModel>>> GetListPaging(GetIconPagingRequest request)
        {
            try
            {
                var query = _context.Icons.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.IconCode.Contains(request.Keyword));
                }
                int totalRow = await query.CountAsync();
                var data = await query
                    .OrderByDescending(x => x.IconId)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new IconViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<IconViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<IconViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<IconViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<IconViewModel>> Create(IconCreateRequest request)
        {
            try
            {
                var is_exists = await _context.Icons
                    .Where(m => m.IconCode.Equals(request.IconCode) && m.IconTypeId == request.IconTypeId && m.IconTypeCode.Equals(request.IconTypeCode))
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<IconViewModel>(ConstantHelper.IconExists);
                }
                var icon = new Icon()
                {
                    IconCode = request.IconCode,
                    IconTypeId = request.IconTypeId,
                    IconTypeCode = request.IconTypeCode,
                };

                _context.Icons.Add(icon);
                await _context.SaveChangesAsync();
                if (icon.IconId <= 0)
                {
                    return new ApiErrorResult<IconViewModel>(ConstantHelper.UpdateError);
                }
                return new ApiSuccessResult<IconViewModel>(new IconViewModel(icon));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<IconViewModel>> Update(IconEditRequest request)
        {
            try
            {
                var is_exists = await _context.Icons
                    .Where(m => m.IconCode.Equals(request.IconCode)
                        && m.IconTypeId == request.IconTypeId
                        && m.IconTypeCode.Equals(request.IconTypeCode)
                        && m.IconId != request.IconId)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<IconViewModel>(ConstantHelper.IconExists);
                }
                var Icon = await _context.Icons.FindAsync(request.IconId);
                if (Icon == null)
                {
                    return new ApiErrorResult<IconViewModel>(ConstantHelper.UpdateNotfound);
                }

                Icon.IconCode = request.IconCode;
                Icon.IconTypeId = request.IconTypeId;
                Icon.IconTypeCode = request.IconTypeCode;
                Icon.StatusId = request.StatusId;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<IconViewModel>(new IconViewModel(Icon));
                }
                else
                {
                    return new ApiErrorResult<IconViewModel>(ConstantHelper.UpdateError);
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
                var Icon = await _context.Icons.FindAsync(id);
                if (Icon == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }
                _context.Icons.Remove(Icon);
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

        public Task GetListPaging(GetUserPagingRequest request)
        {
            throw new NotImplementedException();
        }

    }
}
