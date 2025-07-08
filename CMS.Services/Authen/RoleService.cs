using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.Functions;
using CMS.Models.Authen.RoleClaims;
using CMS.Models.Authen.Roles;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Helper;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetUserMenuRequest = CMS.Models.Authen.Roles.GetUserMenuRequest;

namespace CMS.Services.Authen
{
    public class RoleService:IRoleService
    {
        private readonly AICMSDBContext _context;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _config;
        public RoleService(AICMSDBContext context,
            IConfiguration config,
            RoleManager<Role> roleManager
        )
        {
            _context = context;
            _config = config;
            _roleManager = roleManager;
        }
        public async Task<ApiResult<RoleViewModel>> GetById(int id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return new ApiErrorResult<RoleViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<RoleViewModel>(new RoleViewModel(role));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetListByUser(int userId)
        {
            try
            {
                var roles = (from r in _context.Roles
                             join ur in _context.UserRoles on r.Id equals ur.RoleId
                             where ur.UserId == userId
                             select new RoleViewModel(r)).ToList();
                if (roles == null)
                {
                    return new ApiErrorResult<List<RoleViewModel>>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<List<RoleViewModel>>(roles);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<RoleViewModel>> GetWithClaimsById(int id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return new ApiErrorResult<RoleViewModel>(ConstantHelper.DataNotfound);
                }

                var roleViewModel = new RoleViewModel(role);
                var claims = _context.RoleClaims.Where(m => m.RoleId == id).ToList();
                foreach (var claim in claims)
                {
                    roleViewModel.addClaim(new RoleClaimViewModel(claim));
                }

                return new ApiSuccessResult<RoleViewModel>(roleViewModel);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<RoleViewModel>> GetWithFunctionsById(int id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return new ApiErrorResult<RoleViewModel>(ConstantHelper.DataNotfound);
                }

                var query = _context.Functions.AsNoTracking();
                var functions = await query
                    .Select(x => new FunctionViewModel(x))
                    .ToListAsync();

                functions = FunctionHelper.BuildHỉerachy(functions);

                var r_functions = (from r in _context.Functions
                                   join g in _context.RoleFunctions on r.FunctionId equals g.FunctionId
                                   where g.RoleId == id
                                   select r.FunctionId).ToList();
                functions.Where(m => r_functions.Contains(m.FunctionId))
                    .ToList()
                    .ForEach(m => { m.Selected = true; });

                return new ApiSuccessResult<RoleViewModel>(new RoleViewModel(role, functions));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetAll()
        {
            try
            {
                var query = _roleManager.Roles.AsNoTracking().AsQueryable();

                var data = await query.Select(x => new RoleViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<RoleViewModel>() : data;
                return new ApiSuccessResult<List<RoleViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetHỉerachy(GetRoleHierarchyRequest request)
        {
            try
            {
                var query = _roleManager.Roles.AsNoTracking().AsQueryable();

                /*if (request.ParentRoleId > 0)
                {
                    query = query.Where(m => m.Id == request.ParentRoleId || m.ParentRoleId == request.ParentRoleId || m.LevelId < temp.LevelId);
                }*/
                if (request.IsShow != 2)
                {
                    query = query.Where(m => m.IsShow == request.IsShow);
                }
                if (request.StatusId != 2)
                {
                    query = query.Where(m => m.StatusId == request.StatusId);
                }
                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(m => m.Name.Contains(request.Keyword) || m.Description.Contains(request.Keyword));
                }

                var data = await query
                    .Select(x => new RoleViewModel(x))
                    .ToListAsync();

                data = data == null ? new List<RoleViewModel>() : data;
                var ret_data = RoleHelper.BuildHỉerachy(data, request.ParentRoleId);

                if (request.UserId > 0)
                {
                    var u_roles = await (from r in _context.Roles
                                         join u in _context.UserRoles on r.Id equals u.RoleId
                                         where u.UserId == request.UserId
                                         select r.Id).ToListAsync();
                    ret_data.Where(m => u_roles.Contains(m.Id)).ToList().ForEach(m => { m.Selected = true; });
                }

                return new ApiSuccessResult<List<RoleViewModel>>(ret_data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<RoleViewModel>>> GetUserMenus(GetUserMenuRequest request)
        {
            try
            {
                var roles = await (from r in _context.Roles
                                   join u in _context.UserRoles on r.Id equals u.RoleId
                                   where u.UserId == request.UserId
                                        && r.IsShow == 1
                                        && r.StatusId == 1
                                   select r).ToListAsync();

                var ret_data = BuildUserMenus(roles);

                return new ApiSuccessResult<List<RoleViewModel>>(ret_data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public List<RoleViewModel> BuildUserMenus(List<Role> roles)
        {
            List<RoleViewModel> trees = new List<RoleViewModel>();
            try
            {
                if (roles == null || roles.Count <= 0) return trees;

                var min_level = roles.Min(m => m.LevelId);

                var stack = roles.Where(m => m.LevelId == min_level)
                    .OrderByDescending(m => m.SortOrder)
                    .ToList();

                while (stack.Count > 0)
                {
                    var role = stack[stack.Count - 1];
                    trees.Add(new RoleViewModel(role));
                    stack.RemoveAt(stack.Count - 1);

                    stack.AddRange(roles.Where(m => m.ParentRoleId == role.Id).OrderByDescending(m => m.SortOrder).ToList());
                }

                return trees;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<PagedResult<RoleViewModel>>> GetListPaging(GetRolePagingRequest request)
        {
            try
            {
                var query = _roleManager.Roles.AsNoTracking();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(request.Keyword)
                    || x.Description.Contains(request.Keyword));
                }

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new RoleViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<RoleViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<RoleViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<RoleViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<RoleViewModel>> Create(RoleCreateRequest request)
        {
            try
            {
                request.Controler = string.IsNullOrEmpty(request.Controler) ? string.Empty : request.Controler;
                request.Action = string.IsNullOrEmpty(request.Action) ? string.Empty : request.Action;
                request.Icon = string.IsNullOrEmpty(request.Icon) ? string.Empty : request.Icon;

                var is_exists = await _roleManager.RoleExistsAsync(request.Name);
                if (is_exists)
                {
                    return new ApiErrorResult<RoleViewModel>(ConstantHelper.RoleExists);
                }

                if (request.ParentRoleId <= 0)
                {
                    request.LevelId = 1;
                }
                else
                {
                    request.LevelId = (byte)((await _context.Roles.FindAsync(request.ParentRoleId)).LevelId + 1);
                }

                var role = new Role()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Controler = request.Controler,
                    Action = request.Action,
                    Icon = request.Icon,
                    SortOrder = request.SortOrder,
                    IsShow = request.IsShow,
                    ParentRoleId = request.ParentRoleId,
                    LevelId = request.LevelId,
                    StatusId = request.StatusId
                };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    /*_context.RoleGroupRoles.Add(new RoleGroupRole()
                    {
                        RoleGroupId = ConstantHelper.SysAdmin_RoleGroupId,
                        RoleId = role.Id
                    });

                    var user_ids = _context.UserRoleGroups
                        .Where(m => m.RoleGroupId == ConstantHelper.SysAdmin_RoleGroupId)
                        .Select(m => m.UserId)
                        .ToList();

                    user_ids.ForEach(u =>
                    {
                        _context.UserRoles.Add(new IdentityUserRole<int>()
                        {
                            UserId = u,
                            RoleId = role.Id
                        });
                    });

                    await _context.SaveChangesAsync();*/

                    return new ApiSuccessResult<RoleViewModel>(new RoleViewModel(role));
                }
                return new ApiErrorResult<RoleViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<RoleViewModel>> Update(RoleEditRequest request)
        {
            byte ole_level = 0;
            try
            {
                request.Controler = string.IsNullOrEmpty(request.Controler) ? string.Empty : request.Controler;
                request.Action = string.IsNullOrEmpty(request.Action) ? string.Empty : request.Action;
                request.Icon = string.IsNullOrEmpty(request.Icon) ? string.Empty : request.Icon;

                var is_exists = await _roleManager.Roles
                    .Where(m => m.Name.Equals(request.Name) && m.Id != request.Id)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<RoleViewModel>(ConstantHelper.RoleExists);
                }

                if (request.ParentRoleId <= 0)
                {
                    request.LevelId = 1;
                }
                else
                {
                    request.LevelId = (byte)((await _context.Roles.FindAsync(request.ParentRoleId)).LevelId + 1);
                }

                var role = await _roleManager.FindByIdAsync(request.Id.ToString());
                if (role == null)
                {
                    return new ApiErrorResult<RoleViewModel>(ConstantHelper.UpdateNotfound);
                }

                ole_level = role.LevelId;

                role.Name = request.Name;
                role.Description = request.Description;
                role.Controler = request.Controler;
                role.Action = request.Action;
                role.Icon = request.Icon;
                role.SortOrder = request.SortOrder;
                role.IsShow = request.IsShow;
                role.ParentRoleId = request.ParentRoleId;
                role.LevelId = request.LevelId;
                role.StatusId = request.StatusId;

                if (ole_level != role.LevelId)
                {
                    List<int> parents = new List<int>() { role.Id };
                    byte level = (byte)(role.LevelId + 1);
                    while (parents.Count > 0)
                    {
                        var subs = _context.Roles
                            .Where(m => parents.Contains(m.ParentRoleId))
                            .ToList();
                        if (subs != null && subs.Count > 0)
                        {
                            parents = subs.Select(m => m.Id).ToList();
                            subs.ForEach(x => { x.LevelId = level; });
                            level++;
                        }
                        else
                        {
                            parents = new List<int>();
                        }
                    }
                }

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return new ApiSuccessResult<RoleViewModel>(new RoleViewModel(role));
                }
                else
                {
                    return new ApiErrorResult<RoleViewModel>(ConstantHelper.UpdateError);
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
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                /*_context.RoleGroupRoles
                    .Where(m => m.RoleId == id)
                    .ToList()
                    .ForEach(r =>
                    {
                        _context.RoleGroupRoles.Remove(r);
                    });*/

                _context.UserRoles
                    .Where(m => m.RoleId == id)
                    .ToList()
                    .ForEach(u =>
                    {
                        _context.UserRoles.Remove(u);
                    });

                _context.RoleFunctions
                    .Where(m => m.RoleId == id)
                    .ToList()
                    .ForEach(u =>
                    {
                        _context.RoleFunctions.Remove(u);
                    });

                _context.Roles.Remove(role);

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

        public async Task<ApiResult<string>> AssignFunction(RoleAssignFunctionRequest request)
        {
            try
            {
                var selected_ids = request.Functions
                    .Where(m => m.Selected == true)
                    .Select(m => m.FunctionId)
                    .ToList();

                var curr_ids = _context.RoleFunctions
                    .Where(m => m.RoleId == request.RoleId)
                    .Select(m => m.FunctionId)
                    .ToList();

                var remove_ids = curr_ids.Where(m => !selected_ids.Contains(m)).ToList();

                var add_ids = selected_ids.Where(m => !curr_ids.Contains(m)).ToList();

                if ((remove_ids == null || remove_ids.Count <= 0)
                    && (add_ids == null || add_ids.Count <= 0))
                {
                    return new ApiSuccessResult<string>(ConstantHelper.UpdateSuccess);
                }

                var user_ids = _context.UserRoles
                            .Where(m => m.RoleId == request.RoleId)
                            .Select(m => m.UserId)
                            .ToList();

                if (remove_ids != null && remove_ids.Count > 0)
                {
                    _context.RoleFunctions
                        .Where(m => m.RoleId == request.RoleId
                            && remove_ids.Contains(m.FunctionId))
                        .ToList()
                        .ForEach(m =>
                        {
                            if (user_ids != null && user_ids.Count > 0)
                            {
                                user_ids.ForEach(u =>
                                {
                                    var orther_user_function = (from ur in _context.UserRoles
                                                                join rf in _context.RoleFunctions
                                                                on ur.RoleId equals rf.RoleId
                                                                join uf in _context.UserFunctions
                                                                on rf.FunctionId equals uf.FunctionId
                                                                where ur.UserId == u
                                                                   && uf.FunctionId == m.FunctionId
                                                                   && ur.RoleId != request.RoleId
                                                                select new
                                                                {
                                                                    rf.RoleId,
                                                                    rf.FunctionId,
                                                                    uf.UserId,
                                                                }).FirstOrDefault();

                                    var user_function = _context.UserFunctions.Find(u, m.FunctionId);

                                    if (orther_user_function == null && user_function != null)
                                    {
                                        _context.UserFunctions.Remove(user_function);
                                    }
                                });
                            }

                            _context.RoleFunctions.Remove(m);

                        });
                }

                if (add_ids != null && add_ids.Count > 0)
                {
                    add_ids.ForEach(m =>
                    {
                        _context.RoleFunctions.Add(new RoleFunction()
                        {
                            RoleId = request.RoleId,
                            FunctionId = m
                        });

                        if (user_ids != null && user_ids.Count > 0)
                        {
                            user_ids.ForEach(u =>
                            {
                                if (_context.UserFunctions.Find(u, m) == null)
                                {
                                    _context.UserFunctions.Add(new UserFunction()
                                    {
                                        UserId = u,
                                        FunctionId = m
                                    });
                                }
                            });
                        }
                    });
                }

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<string>(ConstantHelper.UpdateSuccess);
                }
                else
                {
                    return new ApiErrorResult<string>(ConstantHelper.UpdateError);
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
