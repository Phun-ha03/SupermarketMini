using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.UserLogs;
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
    public class UserLogService : IUserLogService
    {
        private readonly AICMSDBContext _context;
        public UserLogService(AICMSDBContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<UserLogViewModel>> GetById(int id)
        {
            try
            {
                var icon = await _context.UserLogs.AsNoTracking().Include(x => x.UserLogDetails).FirstOrDefaultAsync(x => x.UserLogId == id);
                if (icon == null)
                {
                    return new ApiErrorResult<UserLogViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<UserLogViewModel>(new UserLogViewModel(icon));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<List<UserLogViewModel>>> GetAll()
        {
            try
            {
                var query = _context.UserLogs.AsNoTracking().Include(x => x.UserLogDetails);

                var data = await query.Select(x => new UserLogViewModel(x))
                    .ToListAsync();
                data ??= new List<UserLogViewModel>();
                return new ApiSuccessResult<List<UserLogViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<List<UserLogViewModel>>> GetAllActive()
        {
            try
            {
                var query = _context.UserLogs.AsNoTracking().Where(m => m.StatusId == 1);

                var data = await query.Select(x => new UserLogViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<UserLogViewModel>() : data;
                return new ApiSuccessResult<List<UserLogViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<PagedResult<UserLogViewModel>>> GetListPaging(GetUserLogPagingRequest request)
        {
            try
            {
                var query = _context.UserLogs.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    _ = long.TryParse(request.Keyword, out long l1);
                    query = query.Where(x =>
                        x.TableName.Contains(request.Keyword) ||
                        x.TableRowId == l1 ||
                        x.IpAddress.Contains(request.Keyword)
                    );
                }
                int totalRow = await query.CountAsync();
                var data = await query
                    .OrderByDescending(x => x.UserLogId)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Include(x => x.UserLogDetails)
                    .Select(x => new UserLogViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<UserLogViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data ?? new List<UserLogViewModel>()
                };

                return new ApiSuccessResult<PagedResult<UserLogViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<UserLogViewModel>> Create(UserLogCreateRequest request)
        {
            try
            {
                var usLog = new UserLog()
                {

                    UserId = request.UserId,

                    IpAddress = request.IpAddress,

                    ActionId = request.ActionId,

                    TableName = request.TableName,

                    TableRowId = request.TableRowId,

                    CrDateTime = DateTime.Now,

                    StatusId = 0,
                };
                _context.UserLogs.Add(usLog);
                var result = await _context.SaveChangesAsync();
                if (result <= 0)
                {
                    return new ApiErrorResult<UserLogViewModel>(ConstantHelper.UpdateError);
                }
                return new ApiSuccessResult<UserLogViewModel>(new UserLogViewModel(usLog));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<UserLogViewModel>> Update(UserLogEditRequest request)
        {
            try
            {
                var UserLog = await _context.UserLogs.FindAsync(request.UserLogId);
                if (UserLog == null)
                {
                    return new ApiErrorResult<UserLogViewModel>(ConstantHelper.UpdateNotfound);
                }

                UserLog.UserId = request.UserId;
                UserLog.IpAddress = request.IpAddress;
                UserLog.ActionId = request.ActionId;
                UserLog.TableName = request.TableName;
                UserLog.TableRowId = request.TableRowId;
                //UserLog.CrDateTime = DateTime.Now;
                UserLog.StatusId = 0;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<UserLogViewModel>(new UserLogViewModel(UserLog));
                }
                else
                {
                    return new ApiErrorResult<UserLogViewModel>(ConstantHelper.UpdateError);
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
                var Icon = await _context.UserLogs.FindAsync(id);
                if (Icon == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }
                _context.UserLogs.Remove(Icon);
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
