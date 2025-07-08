using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.UserClaims;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen
{
    public class UserClaimService:IUserClaimService
    {
        private readonly AICMSDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        public UserClaimService(AICMSDBContext context,
            IConfiguration config,
            UserManager<User> userManager)
        {
            _context = context;
            _config = config;
            _userManager = userManager;
        }
        public async Task<ApiResult<UserClaimViewModel>> GetById(int id)
        {
            try
            {
                var userClaim = await _context.UserClaims.FindAsync(id);
                if (userClaim == null)
                {
                    return new ApiErrorResult<UserClaimViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<UserClaimViewModel>(new UserClaimViewModel(userClaim));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<UserClaimViewModel>>> GetAll(int userId)
        {
            try
            {
                var query = _context.UserClaims.AsNoTracking();

                var data = await query.Where(m => m.UserId == userId)
                    .Select(x => new UserClaimViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<UserClaimViewModel>() : data;
                return new ApiSuccessResult<List<UserClaimViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<PagedResult<UserClaimViewModel>>> GetListPaging(GetUserClaimPagingRequest request)
        {
            try
            {
                var query = _context.UserClaims.AsNoTracking().Where(m => m.UserId == request.UserId);

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.ClaimType.Contains(request.Keyword)
                    || x.ClaimValue.Contains(request.Keyword));
                }

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new UserClaimViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<UserClaimViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<UserClaimViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<UserClaimViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserClaimViewModel>> Create(UserClaimCreateRequest request)
        {
            try
            {
                var is_exists = await _context.UserClaims
                    .Where(m => m.UserId == request.UserId
                        && m.ClaimType.Equals(request.ClaimType)
                        && m.ClaimValue.Equals(request.ClaimValue))
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<UserClaimViewModel>(ConstantHelper.UserClaimExists);
                }
                var userClaim = new IdentityUserClaim<int>()
                {
                    UserId = request.UserId,
                    ClaimType = request.ClaimType,
                    ClaimValue = request.ClaimValue
                };
                _context.UserClaims.Add(userClaim);
                await _context.SaveChangesAsync();
                if (userClaim.Id <= 0)
                {
                    return new ApiErrorResult<UserClaimViewModel>(ConstantHelper.UpdateError);
                }
                return new ApiSuccessResult<UserClaimViewModel>(new UserClaimViewModel(userClaim));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserClaimViewModel>> Update(UserClaimEditRequest request)
        {
            try
            {
                var is_exists = await _context.UserClaims
                    .Where(m => m.UserId == request.UserId
                        && m.ClaimType.Equals(request.ClaimType)
                        && m.ClaimValue.Equals(request.ClaimValue)
                        && m.Id != request.Id)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<UserClaimViewModel>(ConstantHelper.UserClaimExists);
                }
                var userClaim = await _context.UserClaims.FindAsync(request.Id);
                if (userClaim == null)
                {
                    return new ApiErrorResult<UserClaimViewModel>(ConstantHelper.UpdateNotfound);
                }
                userClaim.ClaimType = request.ClaimType;
                userClaim.ClaimValue = request.ClaimValue;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<UserClaimViewModel>(new UserClaimViewModel(userClaim));
                }
                else
                {
                    return new ApiErrorResult<UserClaimViewModel>(ConstantHelper.UpdateError);
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
                var userClaim = await _context.UserClaims.FindAsync(id);
                if (userClaim == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }
                _context.UserClaims.Remove(userClaim);
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
