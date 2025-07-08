using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Data.Enums.Authen;
using CMS.Models.Authen.UserStatuses;
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
    public class UserStatusService:IUserStatusService
    {
        private readonly AICMSDBContext _context;
        public UserStatusService(AICMSDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<UserStatusViewModel>> GetById(byte id)
        {
            try
            {
                var UserStatus = await _context.UserStatuses.FindAsync(id);
                if (UserStatus == null)
                {
                    return new ApiErrorResult<UserStatusViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<UserStatusViewModel>(new UserStatusViewModel(UserStatus));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<UserStatusViewModel>> GetByIdMax()
        {
            try
            {
                var userStatus = await _context.UserStatuses.AsNoTracking()
                    .OrderByDescending(x => x.UserStatusId)
                    .Take(1).FirstOrDefaultAsync();
                if (userStatus == null || userStatus.UserStatusId <= 0)
                {
                    return new ApiSuccessResult<UserStatusViewModel>(new UserStatusViewModel());
                }
                else
                {
                    return new ApiSuccessResult<UserStatusViewModel>(new UserStatusViewModel(userStatus));
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<List<UserStatusViewModel>>> GetAll()
        {
            try
            {
                var query = _context.UserStatuses.AsNoTracking()
                    .Where(m => m.UserStatusId != (byte)UserStatusEnum.Deleted);

                var data = await query.Select(x => new UserStatusViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<UserStatusViewModel>() : data;
                return new ApiSuccessResult<List<UserStatusViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<PagedResult<UserStatusViewModel>>> GetListPaging(GetUserStatusPagingRequest request)
        {
            try
            {
                var query = _context.UserStatuses.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.UserStatusName.Contains(request.Keyword) || x.UserStatusDesc.Contains(request.Keyword));
                }

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new UserStatusViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<UserStatusViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<UserStatusViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<UserStatusViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<UserStatusViewModel>> Create(UserStatusCreateRequest request)
        {
            try
            {
                var is_exists = await _context.UserStatuses
                    .Where(m => m.UserStatusName.Equals(request.UserStatusName))
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<UserStatusViewModel>(ConstantHelper.UserStatusExists);
                }
                var userStatus = new UserStatus()
                {
                    UserStatusId = request.UserStatusId,
                    UserStatusName = request.UserStatusName,
                    UserStatusDesc = request.UserStatusDesc,
                    Color = request.Color,
                    BackgroundColor = request.BackgroundColor
                };
                _context.UserStatuses.Add(userStatus);
                await _context.SaveChangesAsync();
                if (userStatus.UserStatusId <= 0)
                {
                    return new ApiErrorResult<UserStatusViewModel>(ConstantHelper.UpdateError);
                }
                return new ApiSuccessResult<UserStatusViewModel>(new UserStatusViewModel(userStatus));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<UserStatusViewModel>> Update(UserStatusEditRequest request)
        {
            try
            {
                var is_exists = await _context.UserStatuses
                    .Where(m => m.UserStatusName.Equals(request.UserStatusName)
                        && m.UserStatusId != request.UserStatusId)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<UserStatusViewModel>(ConstantHelper.UserStatusExists);
                }
                var UserStatus = await _context.UserStatuses.FindAsync(request.UserStatusId);
                if (UserStatus == null)
                {
                    return new ApiErrorResult<UserStatusViewModel>(ConstantHelper.UpdateNotfound);
                }
                UserStatus.UserStatusName = request.UserStatusName;
                UserStatus.UserStatusDesc = request.UserStatusDesc;
                UserStatus.Color = request.Color;
                UserStatus.BackgroundColor = request.BackgroundColor;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<UserStatusViewModel>(new UserStatusViewModel(UserStatus));
                }
                else
                {
                    return new ApiErrorResult<UserStatusViewModel>(ConstantHelper.UpdateError);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<int>> Delete(byte id)
        {
            try
            {
                var UserStatus = await _context.UserStatuses.FindAsync(id);
                if (UserStatus == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }
                _context.UserStatuses.Remove(UserStatus);
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
