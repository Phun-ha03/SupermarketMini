using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.Functions;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Helper;
using CMS.Utilities.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen
{
    public class FunctionService :IFunctionService 
    {
        private readonly AICMSDBContext _context;

        public FunctionService(AICMSDBContext context
        )
        {
            _context = context;
        }

        public async Task<ApiResult<FunctionViewModel>> GetById(int id)
        {
            try
            {
                var function = await _context.Functions.FindAsync(id);
                if (function == null)
                {
                    return new ApiErrorResult<FunctionViewModel>(ConstantHelper.DataNotfound);
                }
                return new ApiSuccessResult<FunctionViewModel>(new FunctionViewModel(function));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<FunctionViewModel>>> GetAll()
        {
            try
            {
                var query = _context.Functions.AsNoTracking().AsQueryable();

                var data = await query.Select(x => new FunctionViewModel(x))
                    .ToListAsync();
                data = data == null ? new List<FunctionViewModel>() : data;
                return new ApiSuccessResult<List<FunctionViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<FunctionViewModel>>> GetHỉerachy(GetFunctionHierarchyRequest request)
        {
            try
            {
                var query = _context.Functions.AsNoTracking().AsQueryable();

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
                    .Select(x => new FunctionViewModel(x))
                    .ToListAsync();

                data = data == null ? new List<FunctionViewModel>() : data;
                var ret_data = FunctionHelper.BuildHỉerachy(data, request.ParentFunctionId);

                if (request.UserId > 0)
                {
                    var u_functions = await (from r in _context.Functions
                                             join u in _context.UserFunctions on r.FunctionId equals u.FunctionId
                                             where u.UserId == request.UserId
                                             select r.FunctionId).ToListAsync();
                    ret_data.Where(m => u_functions.Contains(m.FunctionId)).ToList().ForEach(m => { m.Selected = true; });
                }
                if (request.RoleId > 0)
                {
                    var g_functions = await (from r in _context.Functions
                                             join g in _context.RoleFunctions on r.FunctionId equals g.FunctionId
                                             where g.RoleId == request.RoleId
                                             select r.FunctionId).ToListAsync();
                    ret_data.Where(m => g_functions.Contains(m.FunctionId)).ToList().ForEach(m => { m.Selected = true; });
                }

                return new ApiSuccessResult<List<FunctionViewModel>>(ret_data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<FunctionViewModel>>> GetUserMenus(GetUserMenuRequest request)
        {
            try
            {
                var functions = await (from r in _context.Functions
                                       join u in _context.UserFunctions on r.FunctionId equals u.FunctionId
                                       where u.UserId == request.UserId
                                            && r.IsShow == 1
                                            && r.StatusId == 1
                                       select r).ToListAsync();

                var ret_data = BuildUserMenus(functions);

                return new ApiSuccessResult<List<FunctionViewModel>>(ret_data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<List<FunctionViewModel>>> GetAllActiveUserMenus(GetUserMenuRequest request)
        {
            try
            {
                var functions = await (from r in _context.Functions
                                       join u in _context.UserFunctions on r.FunctionId equals u.FunctionId
                                       where u.UserId == request.UserId
                                            && r.StatusId == 1
                                       select r).ToListAsync();

                var ret_data = BuildUserMenus(functions);

                return new ApiSuccessResult<List<FunctionViewModel>>(ret_data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public List<FunctionViewModel> BuildUserMenus(List<Function> functions)
        {
            List<FunctionViewModel> trees = new List<FunctionViewModel>();
            try
            {
                if (functions == null || functions.Count <= 0) return trees;

                var min_level = functions.Min(m => m.LevelId);

                var stack = functions.Where(m => m.LevelId == min_level)
                    .OrderByDescending(m => m.SortOrder)
                    .ToList();

                while (stack.Count > 0)
                {
                    var function = stack[stack.Count - 1];
                    trees.Add(new FunctionViewModel(function));
                    stack.RemoveAt(stack.Count - 1);

                    stack.AddRange(functions.Where(m => m.ParentFunctionId == function.FunctionId).OrderByDescending(m => m.SortOrder).ToList());
                }

                return trees;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<FunctionViewModel>> Create(FunctionCreateRequest request)
        {
            try
            {
                request.Controler = string.IsNullOrEmpty(request.Controler) ? string.Empty : request.Controler;
                request.Action = string.IsNullOrEmpty(request.Action) ? string.Empty : request.Action;
                request.Icon = string.IsNullOrEmpty(request.Icon) ? string.Empty : request.Icon;

                var is_exists = (await _context.Functions.Where(m => m.Name.Equals(request.Name))
                    .ToListAsync()).Any();
                if (is_exists)
                {
                    return new ApiErrorResult<FunctionViewModel>(ConstantHelper.FunctionExists);
                }

                if (request.ParentFunctionId <= 0)
                {
                    request.LevelId = 1;
                }
                else
                {
                    request.LevelId = (byte)((await _context.Functions.FindAsync(request.ParentFunctionId)).LevelId + 1);
                }

                var function = new Function()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Controler = request.Controler,
                    Action = request.Action,
                    Icon = request.Icon,
                    SortOrder = request.SortOrder,
                    IsShow = request.IsShow,
                    ParentFunctionId = request.ParentFunctionId,
                    LevelId = request.LevelId,
                    CrDateTime = DateTime.Now,
                    StatusId = request.StatusId
                };

                _context.Functions.Add(function);

                var role = await _context.Roles.FindAsync(ConstantHelper.SysAdmin_RoleId);
                //_roleManager.FindByIdAsync(ConstantHelper.SysAdmin_RoleId.ToString());
                if (role != null && role.Id > 0)
                {
                    var roleFunction = new RoleFunction()
                    {
                        RoleId = role.Id,
                        Function = function
                    };
                    _context.RoleFunctions.Add(roleFunction);

                    var user_ids = _context.UserRoles
                        .Where(m => m.RoleId == role.Id)
                        .Select(m => m.UserId)
                        .ToList();

                    user_ids.ForEach(u =>
                    {
                        _context.UserFunctions.Add(new UserFunction()
                        {
                            UserId = u,
                            Function = function
                        });
                    });
                }

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<FunctionViewModel>(new FunctionViewModel(function));
                }
                return new ApiErrorResult<FunctionViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<FunctionViewModel>> Update(FunctionEditRequest request)
        {
            byte ole_level = 0;
            try
            {
                request.Controler = string.IsNullOrEmpty(request.Controler) ? string.Empty : request.Controler;
                request.Action = string.IsNullOrEmpty(request.Action) ? string.Empty : request.Action;
                request.Icon = string.IsNullOrEmpty(request.Icon) ? string.Empty : request.Icon;

                var is_exists = await _context.Functions
                    .Where(m => m.Name.Equals(request.Name) && m.FunctionId != request.FunctionId)
                    .AnyAsync();
                if (is_exists)
                {
                    return new ApiErrorResult<FunctionViewModel>(ConstantHelper.FunctionExists);
                }

                if (request.ParentFunctionId <= 0)
                {
                    request.LevelId = 1;
                }
                else
                {
                    request.LevelId = (byte)((await _context.Functions.FindAsync(request.ParentFunctionId)).LevelId + 1);
                }

                var function = await _context.Functions.FindAsync(request.FunctionId);
                if (function == null)
                {
                    return new ApiErrorResult<FunctionViewModel>(ConstantHelper.UpdateNotfound);
                }

                ole_level = function.LevelId;

                function.Name = request.Name;
                function.Description = request.Description;
                function.Controler = request.Controler;
                function.Action = request.Action;
                function.Icon = request.Icon;
                function.SortOrder = request.SortOrder;
                function.IsShow = request.IsShow;
                function.ParentFunctionId = request.ParentFunctionId;
                function.LevelId = request.LevelId;
                function.StatusId = request.StatusId;

                if (ole_level != function.LevelId)
                {
                    List<int> parents = new List<int>() { function.FunctionId };
                    byte level = (byte)(function.LevelId + 1);
                    while (parents.Count > 0)
                    {
                        var subs = _context.Functions
                            .Where(m => parents.Contains(m.ParentFunctionId))
                            .ToList();
                        if (subs != null && subs.Count > 0)
                        {
                            parents = subs.Select(m => m.FunctionId).ToList();
                            subs.ForEach(x => { x.LevelId = level; });
                            level++;
                        }
                        else
                        {
                            parents = new List<int>();
                        }
                    }
                }

                _context.Entry(function).State = EntityState.Modified;

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ApiSuccessResult<FunctionViewModel>(new FunctionViewModel(function));
                }
                else
                {
                    return new ApiErrorResult<FunctionViewModel>(ConstantHelper.UpdateError);
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
                var function = await _context.Functions.FindAsync(id);
                if (function == null)
                {
                    return new ApiErrorResult<int>(ConstantHelper.DeleteNotfound);
                }

                _context.RoleFunctions
                    .Where(m => m.FunctionId == id)
                    .ToList()
                    .ForEach(r =>
                    {
                        _context.RoleFunctions.Remove(r);
                    });

                _context.UserFunctions
                    .Where(m => m.FunctionId == id)
                    .ToList()
                    .ForEach(u =>
                    {
                        _context.UserFunctions.Remove(u);
                    });

                _context.Functions.Remove(function);

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

        public async Task<ApiResult<bool>> CheckPermission(int userId, string controller, string action)
        {
            try
            {
                var function = await (from f in _context.Functions
                                      join u in _context.UserFunctions on f.FunctionId equals u.FunctionId
                                      where u.UserId == userId
                                           && f.Controler.ToLower().Equals(controller)
                                           && f.Action.ToLower().Equals(action)
                                      select f).FirstOrDefaultAsync();

                if (function != null && function.FunctionId > 0)
                {
                    return new ApiSuccessResult<bool>(true);
                }
                return new ApiSuccessResult<bool>(false);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
    }
}

