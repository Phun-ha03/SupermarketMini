using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.RoleClaims;
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
    public class RoleClaimService:IRoleClaimService
    {
        private readonly AICMSDBContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _config;
        public RoleClaimService(AICMSDBContext context,
            IConfiguration config,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _config = config;
            _roleManager = roleManager;
        }
        public async Task<ApiResult<RoleClaimViewModel>> GetById(int id)
        {
            try
            {
                var roleClaim = await _context.RoleClaims.FindAsync(id);
                if (roleClaim == null)
                {
                    return new ApiErrorResult<RoleClaimViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<RoleClaimViewModel>(new RoleClaimViewModel(roleClaim));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<RoleClaimViewModel>>> GetAll(int roleId)
        {
            try
            {
                var query = _context.RoleClaims.AsNoTracking();

                var data = await query.Where(m => m.RoleId == roleId)
                    .Select(x => new RoleClaimViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<RoleClaimViewModel>() : data;
                return new ApiSuccessResult<List<RoleClaimViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<PagedResult<RoleClaimViewModel>>> GetListPaging(GetRoleClaimPagingRequest request)
        {
            try
            {
                var query = _context.RoleClaims.AsNoTracking().Where(m => m.RoleId == request.RoleId);

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.ClaimType.Contains(request.Keyword)
                    || x.ClaimValue.Contains(request.Keyword));
                }

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new RoleClaimViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<RoleClaimViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<RoleClaimViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<RoleClaimViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<RoleClaimViewModel>> Create(RoleClaimCreateRequest request)
        {
            try
            {
                var is_exists = await _context.RoleClaims
                    .Where(m => m.RoleId == request.RoleId
                        && m.ClaimType.Equals(request.ClaimType)
                        && m.ClaimValue.Equals(request.ClaimValue))
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<RoleClaimViewModel>(ConstantHelper.RoleClaimExists);
                }

                var roleClaim = new IdentityRoleClaim<int>()
                {
                    //RoleId = request.RoleId,
                    ClaimType = request.ClaimType,
                    ClaimValue = request.ClaimValue
                };
                _context.RoleClaims.Add(roleClaim);
                await _context.SaveChangesAsync();
                if (roleClaim.Id <= 0)
                {
                    return new ApiErrorResult<RoleClaimViewModel>(ConstantHelper.UpdateError);
                }
                return new ApiSuccessResult<RoleClaimViewModel>(new RoleClaimViewModel(roleClaim));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<RoleClaimViewModel>> Update(RoleClaimEditRequest request)
        {
            try
            {
                var is_exists = await _context.RoleClaims
                    .Where(m => m.RoleId == request.RoleId
                        && m.ClaimType.Equals(request.ClaimType)
                        && m.ClaimValue.Equals(request.ClaimValue)
                        && m.Id != request.Id)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<RoleClaimViewModel>(ConstantHelper.RoleClaimExists);
                }

                var roleClaim = await _context.RoleClaims.FindAsync(request.Id);
                if (roleClaim == null)
                {
                    return new ApiErrorResult<RoleClaimViewModel>(ConstantHelper.UpdateNotfound);
                }
                roleClaim.ClaimType = request.ClaimType;
                roleClaim.ClaimValue = request.ClaimValue;
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<RoleClaimViewModel>(new RoleClaimViewModel(roleClaim));
                }
                else
                {
                    return new ApiErrorResult<RoleClaimViewModel>(ConstantHelper.UpdateError);
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
                var roleClaim = await _context.RoleClaims.FindAsync(id);
                if (roleClaim == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }
                _context.RoleClaims.Remove(roleClaim);
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
