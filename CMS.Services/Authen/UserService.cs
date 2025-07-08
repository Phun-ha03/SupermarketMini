using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Authen;
using CMS.Data.Enums.Authen;
using CMS.Models.Authen.Functions;
using CMS.Models.Authen.Roles;
using CMS.Models.Authen.Users;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Helper;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Services.Authen
{
    public class UserService:IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly AICMSDBContext _context;

        public UserService(UserManager<User> userManager
            , SignInManager<User> signInManager
            , IConfiguration config
            , AICMSDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _context = context;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    return new ApiErrorResult<string>(ConstantHelper.AccountNotfound);
                }
                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);
                if (!result.Succeeded)
                {
                    return new ApiErrorResult<string>(ConstantHelper.PasswordWrong);
                }

                var roles = await _userManager.GetRolesAsync(user);

                /*var role_trees = (await _roleService.GetUserMenus(new ICSoft.Jobman.Models.Authen.Roles.GetUserMenuRequest()
                {
                    UserId = user.Id
                })).ResultObj;

                role_trees = role_trees == null ? new List<RoleViewModel>() : role_trees;*/

                var claims = new[] {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FullName),
                    new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(new UserViewModel(user))),
                    new Claim(ClaimTypes.Role, string.Join(";", roles))
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds
                    );

                string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

                return new ApiSuccessResult<string>(tokenStr);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserViewModel>> AuthenticateV2(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                /*var user = await _context.Users.AsNoTracking()
                    .Where(m => m.UserName.Equals(request.UserName)
                            && m.UserStatusId != (byte)UserStatusEnum.Deleted)
                    .FirstOrDefaultAsync();*/

                if (user == null)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountNotfound);
                }

                if (user.UserStatusId == (byte)UserStatusEnum.InActive)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountInActive);
                }
                if (user.UserStatusId == (byte)UserStatusEnum.Suspend)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountSuspend);
                }
                if (user.UserStatusId == (byte)UserStatusEnum.Locked)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountLocked);
                }
                if (user.UserStatusId == (byte)UserStatusEnum.Deleted)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountDeleted);
                }

                if (user.UserStatusId != (byte)UserStatusEnum.Active)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountInvalid);
                }

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);
                if (!result.Succeeded)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.PasswordWrong);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var roles_str = roles == null || roles.Count <= 0 ? string.Empty : string.Join(";", roles);

                var userViewModel = new UserViewModel(user);
                userViewModel.Roles = roles_str;

                return new ApiSuccessResult<UserViewModel>(userViewModel);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<string>> SignOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new ApiSuccessResult<string>("Signed out");
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserViewModel>> Register(RegisterRequest request)
        {
            try
            {
                //var user = await _userManager.FindByNameAsync(request.UserName);
                var user = await _context.Users
                    .Where(m => m.UserName.Equals(request.UserName)
                            && m.UserStatusId != (byte)UserStatusEnum.Deleted)
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.UserNameExists);
                }
                if (!string.IsNullOrEmpty(request.Email))
                {
                    user = await _userManager.FindByEmailAsync(request.Email);
                    if (user != null)
                    {
                        return new ApiErrorResult<UserViewModel>(ConstantHelper.EmailExists);
                    }
                }

                user = new User()
                {
                    UserName = request.UserName,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    FullName = request.FullName,
                    Avatar = string.Empty,
                    GenderId = request.GenderId,
                    DateOfBirth = request.DateOfBirth,
                    Address = string.Empty,
                    Comments = string.Empty,
                    OAuthId = string.Empty,
                    OAuthName = string.Empty,
                    CrDateTime = DateTime.Now,
                    ActiveDateTime = DateTime.Now,
                    Color = request.Color,
                    Background = request.Background,
                    ShopId = request.ShopId,
                    UserStatusId = (byte)UserStatusEnum.Active
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return new ApiSuccessResult<UserViewModel>(new UserViewModel(user));
                }

                if (result.Errors != null)
                {
                    string errors = string.Empty;
                    foreach (var err in result.Errors)
                    {
                        if (!string.IsNullOrEmpty(errors)) errors += "; ";
                        errors += err.Description;
                    }
                    return new ApiErrorResult<UserViewModel>(errors);
                }
                return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserViewModel>> Create(UserCreateRequest request)
        {
            try
            {
                //var user = await _userManager.FindByNameAsync(request.UserName);
                var user = await _context.Users
                    .Where(m => m.UserName.Equals(request.UserName))
                    .FirstOrDefaultAsync();

                if (user != null)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.UserNameExists);
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    user = await _context.Users
                        .Where(m => m.Email.Equals(request.Email))
                        .FirstOrDefaultAsync();

                    if (user != null)
                    {
                        return new ApiErrorResult<UserViewModel>(ConstantHelper.EmailExists);
                    }
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    user = await _userManager.FindByEmailAsync(request.Email);
                    if (user != null)
                    {
                        return new ApiErrorResult<UserViewModel>(ConstantHelper.EmailExists);
                    }
                }

                user = new User()
                {
                    UserName = request.UserName,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    FullName = request.FullName,
                    Avatar = string.Empty,
                    GenderId = request.GenderId <= 0 ? null : request.GenderId,
                    DateOfBirth = request.DateOfBirth,
                    Address = request.Address,
                    Comments = string.Empty,
                    OAuthId = string.Empty,
                    OAuthName = string.Empty,
                    CrDateTime = DateTime.Now,
                    ActiveDateTime = DateTime.Now,
                    Color = request.Color,
                    Background = request.Background,
                    ShopId = request.ShopId <= 0 ? null : request.ShopId,
                    UserStatusId = (byte)UserStatusEnum.Active
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    if (user.ShopId > 0)
                    {
                        try
                        {
                            _context.UserRoles.Add(new IdentityUserRole<int>()
                            {
                                UserId = user.Id,
                                RoleId = ConstantHelper.ShopStaffRoleId
                            });

                            var functions = await _context.RoleFunctions.AsNoTracking()
                                .Where(m => m.RoleId == ConstantHelper.ShopStaffRoleId)
                                .Select(m => m.FunctionId)
                                .ToListAsync();

                            if (functions != null && functions.Count > 0)
                            {
                                functions.ForEach(m =>
                                {
                                    _context.UserFunctions.Add(new UserFunction()
                                    {
                                        UserId = user.Id,
                                        FunctionId = m
                                    });
                                });
                            }

                            await _context.SaveChangesAsync();
                        }
                        catch (Exception ee)
                        {
                            LogHelper.writeLog(ee.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                        }
                    }

                    return new ApiSuccessResult<UserViewModel>(new UserViewModel(user));
                }

                if (result.Errors != null)
                {
                    string errors = string.Empty;
                    foreach (var err in result.Errors)
                    {
                        if (!string.IsNullOrEmpty(errors)) errors += "; ";
                        errors += err.Description;
                    }
                    return new ApiErrorResult<UserViewModel>(errors);
                }
                return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserViewModel>> Update(UserEditRequest request)
        {
            try
            {
                if (!string.IsNullOrEmpty(request.Email))
                {
                    var userExists = await _context.Users
                        .Where(m => m.Email.Equals(request.Email)
                                && m.Id != request.Id)
                        .FirstOrDefaultAsync();

                    if (userExists != null)
                    {
                        return new ApiErrorResult<UserViewModel>(ConstantHelper.EmailExists);
                    }
                }

                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateNotfound);
                }

                //user.Id = request.Id;
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.FullName = request.FullName;
                user.GenderId = request.GenderId <= 0 ? null : request.GenderId;
                user.DateOfBirth = request.DateOfBirth;
                user.Address = request.Address;
                user.Comments = request.Comments;
                user.UserStatusId = request.UserStatusId;
                user.Intro = request.Intro;
                user.NormalizedEmail = request.Email;
                //user.GenderId = request.GenderId;
                user.Color = request.Color;
                user.Background = request.Background;
                if (request.Avatar != null && request.Avatar.Length > 0)
                {
                    user.Avatar = request.Avatar;
                }
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ApiSuccessResult<UserViewModel>(new UserViewModel(user));
                }

                if (result.Errors != null)
                {
                    string errors = string.Empty;
                    foreach (var err in result.Errors)
                    {
                        if (!string.IsNullOrEmpty(errors)) errors += "; ";
                        errors += err.Description;
                    }
                    return new ApiErrorResult<UserViewModel>(errors);
                }
                return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<UserViewModel>> ChangePassword(ChangePasswordRequest request)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountNotfound);
                }


                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

                if (result.Succeeded)
                {
                    return new ApiSuccessResult<UserViewModel>(new UserViewModel(user));
                }

                if (result.Errors != null)
                {
                    string errors = string.Empty;
                    foreach (var err in result.Errors)
                    {
                        if (!string.IsNullOrEmpty(errors)) errors += "; ";
                        errors += err.Description;
                    }
                    return new ApiErrorResult<UserViewModel>(errors);
                }
                return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<UserViewModel>> ResetPassword(ResetPasswordRequest request)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.AccountNotfound);
                }

                var token = request.Token;
                if (string.IsNullOrEmpty(token))
                {
                    token = await _userManager.GeneratePasswordResetTokenAsync(user);
                }

                var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

                if (result.Succeeded)
                {
                    return new ApiSuccessResult<UserViewModel>(new UserViewModel(user));
                }

                if (result.Errors != null)
                {
                    string errors = string.Empty;
                    foreach (var err in result.Errors)
                    {
                        if (!string.IsNullOrEmpty(errors)) errors += "; ";
                        errors += err.Description;
                    }
                    return new ApiErrorResult<UserViewModel>(errors);
                }
                return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<string>> ForgotPassword(ForgotPassWordRequest request)
        {
            try
            {
                //var user = await _userManager.FindByEmailAsync(request.Email);
                var user = await _context.Users
                    .Where(m => m.Email.Equals(request.Email))
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    return new ApiErrorResult<string>(ConstantHelper.AccountNotfound);
                }

                if (user.UserStatusId == (byte)UserStatusEnum.InActive)
                {
                    return new ApiErrorResult<string>(ConstantHelper.AccountInActive);
                }
                if (user.UserStatusId == (byte)UserStatusEnum.Suspend)
                {
                    return new ApiErrorResult<string>(ConstantHelper.AccountSuspend);
                }
                if (user.UserStatusId == (byte)UserStatusEnum.Locked)
                {
                    return new ApiErrorResult<string>(ConstantHelper.AccountLocked);
                }
                if (user.UserStatusId == (byte)UserStatusEnum.Deleted)
                {
                    return new ApiErrorResult<string>(ConstantHelper.AccountDeleted);
                }

                if (user.UserStatusId != (byte)UserStatusEnum.Active)
                {
                    return new ApiErrorResult<string>(ConstantHelper.AccountInvalid);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                token = HttpUtility.UrlEncode(token);

                return new ApiSuccessResult<string>($"u={user.Id}&token={token}");
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserViewModel>> UpdateProfile(UpdateProfileRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateNotfound);
                }

                user.Id = request.Id;
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
                user.FullName = request.FullName;
                user.GenderId = request.GenderId;
                user.DateOfBirth = request.DateOfBirth;
                user.Address = request.Address;
                user.Intro = request.Intro;
                user.NormalizedEmail = request.Email;
                user.Color = request.Color;
                user.Background = request.Background;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ApiSuccessResult<UserViewModel>(new UserViewModel(user));
                }

                if (result.Errors != null)
                {
                    string errors = string.Empty;
                    foreach (var err in result.Errors)
                    {
                        if (!string.IsNullOrEmpty(errors)) errors += "; ";
                        errors += err.Description;
                    }
                    return new ApiErrorResult<UserViewModel>(errors);
                }
                return new ApiErrorResult<UserViewModel>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<string>> ChangeAvatar(ChangeAvatarRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<string>(ConstantHelper.UpdateNotfound);
                }

                user.Avatar = request.Avatar;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ApiSuccessResult<string>(ConstantHelper.UpdateSuccess);
                }

                return new ApiErrorResult<string>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<string>> ChangeCoverPhoto(ChangeCoverPhotoRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<string>(ConstantHelper.UpdateNotfound);
                }

                user.CoverPhoto = request.CoverPhoto;
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ApiSuccessResult<string>(ConstantHelper.UpdateSuccess);
                }

                return new ApiErrorResult<string>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<bool>> Delete(int userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new ApiErrorResult<bool>(ConstantHelper.DeleteNotfound);
                }

                _context.UserRoles
                    .Where(m => m.UserId == userId)
                    .ToList()
                    .ForEach(u =>
                    {
                        _context.UserRoles.Remove(u);
                    });

                _context.UserFunctions
                    .Where(m => m.UserId == userId)
                    .ToList()
                    .ForEach(u =>
                    {
                        _context.UserFunctions.Remove(u);
                    });

                _context.Users.Remove(user);


                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<bool>(true);
                }

                

                return new ApiErrorResult<bool>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserViewModel>> GetById(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserViewModel>(ConstantHelper.DataNotfound);
            }
            return new ApiSuccessResult<UserViewModel>(new UserViewModel(user));
        }

        public async Task<ApiResult<UserViewModel>> GetByIdToDelete(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserViewModel>(ConstantHelper.DataNotfound);
            }

            var userVM = new UserViewModel(user);
            userVM.Deleteable = true;

            return new ApiSuccessResult<UserViewModel>(userVM);
        }
        public async Task<ApiResult<PagedResult<UserViewModel>>> GetListPaging(GetUserPagingRequest request)
        {
            try
            {
                var query = _userManager.Users.AsNoTracking()
                    .Where(x => x.UserStatusId != (byte)UserStatusEnum.Deleted);

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    string keyword = request.Keyword.ToLower();
                    query = query.Where(x => x.FullName.ToLower().Contains(keyword)
                                || x.FirstName.ToLower().Contains(keyword)
                                || x.LastName.ToLower().Contains(keyword)
                                || x.UserName.ToLower().Contains(keyword)
                                || x.Email.ToLower().Contains(keyword)
                                || x.PhoneNumber.ToLower().Contains(keyword));
                }

                int totalRow = await query.CountAsync();
                var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new UserViewModel(x))
                    .ToListAsync();
                var pageResult = new PagedResult<UserViewModel>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data == null ? new List<UserViewModel>() : data
                };

                return new ApiSuccessResult<PagedResult<UserViewModel>>(pageResult);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<List<UserViewModel>>> GetAll()
        {
            try
            {
                var query = _userManager.Users.AsNoTracking()
                    .Where(x => x.UserStatusId != (byte)UserStatusEnum.Deleted);

                var data = await query.Select(x => new UserViewModel(x)).ToListAsync();
                if (data == null)
                {
                    data = new List<UserViewModel>();
                }

                return new ApiSuccessResult<List<UserViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<List<UserViewModel>>> GetByIdList(List<int> ids)
        {
            try
            {
                var query = _userManager.Users.AsNoTracking()
                    .Where(x => ids.Contains(x.Id));

                var data = await query.Select(x => new UserViewModel(x)).ToListAsync();
                if (data == null)
                {
                    data = new List<UserViewModel>();
                }

                return new ApiSuccessResult<List<UserViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task<ApiResult<List<UserViewModel>>> GetAllActive()
        {
            try
            {
                var query = _userManager.Users.AsNoTracking()
                    .Where(x => x.UserStatusId == (byte)UserStatusEnum.Active);

                var data = await query.Select(x => new UserViewModel(x)).ToListAsync();
                if (data == null)
                {
                    data = new List<UserViewModel>();
                }

                return new ApiSuccessResult<List<UserViewModel>>(data);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<UserAssignRoleRequest>> GetUserRoles(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserAssignRoleRequest>(ConstantHelper.DataNotfound);
            }

            var roles = await _context.Roles
                .Select(m => new RoleViewModel(m))
                .ToListAsync();
            var userRoles = await _context.UserRoles
                .Where(m => m.UserId == userId)
                .Select(m => m.RoleId)
                .ToListAsync();

            roles.Where(m => userRoles.Contains(m.Id))
                .ToList()
                .ForEach(m => { m.Selected = true; });

            roles = RoleHelper.BuildHỉerachy(roles);

            var resultObj = new UserAssignRoleRequest()
            {
                UserId = user.Id,
                Fullname = user.FullName,
                Roles = roles
            };
            return new ApiSuccessResult<UserAssignRoleRequest>(resultObj);
        }

        public async Task<ApiResult<UserAssignFunctionRequest>> GetUserFunctions(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return new ApiErrorResult<UserAssignFunctionRequest>(ConstantHelper.DataNotfound);
            }

            var functions = await _context.Functions
                .Select(m => new FunctionViewModel(m))
                .ToListAsync();
            var userFunctions = await _context.UserFunctions
                .Where(m => m.UserId == userId)
                .Select(m => m.FunctionId)
                .ToListAsync();

            functions.Where(m => userFunctions.Contains(m.FunctionId))
                .ToList()
                .ForEach(m => { m.Selected = true; });

            functions = FunctionHelper.BuildHỉerachy(functions);

            var resultObj = new UserAssignFunctionRequest()
            {
                UserId = user.Id,
                Fullname = user.FullName,
                Functions = functions
            };
            return new ApiSuccessResult<UserAssignFunctionRequest>(resultObj);
        }

        public async Task<ApiResult<bool>> AssignRole(UserAssignRoleRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<bool>(ConstantHelper.AccountNotfound);
                }

                var selected_ids = request.Roles
                    .Where(m => m.Selected == true)
                    .Select(m => m.Id)
                    .ToList();

                var curr_ids = _context.UserRoles
                    .Where(m => m.UserId == request.UserId)
                    .Select(m => m.RoleId)
                    .ToList();

                var remove_ids = curr_ids.Where(m => !selected_ids.Contains(m)).ToList();

                var add_ids = selected_ids.Where(m => !curr_ids.Contains(m)).ToList();

                if ((remove_ids == null || remove_ids.Count <= 0)
                    && (add_ids == null || add_ids.Count <= 0))
                {
                    return new ApiSuccessResult<bool>(true);
                }

                var hold_funcs = (from f in _context.Functions
                                  join rf in _context.RoleFunctions on f.FunctionId equals rf.FunctionId
                                  where selected_ids.Contains(rf.RoleId)
                                  select f.FunctionId).ToList();

                if (remove_ids != null && remove_ids.Count > 0)
                {
                    remove_ids.ForEach(m =>
                    {
                        _context.UserRoles.Remove(new IdentityUserRole<int>()
                        {
                            UserId = request.UserId,
                            RoleId = m
                        });
                    });

                    var r_functions = _context.RoleFunctions
                        .Where(m => remove_ids.Contains(m.RoleId) && !hold_funcs.Contains(m.FunctionId))
                        .Select(m => m.FunctionId).ToList();

                    if (r_functions != null && r_functions.Count > 0)
                    {
                        var del_user_funcs = _context.UserFunctions
                            .Where(m => m.UserId == request.UserId && r_functions.Contains(m.FunctionId))
                            .ToList();
                        if (del_user_funcs != null && del_user_funcs.Count > 0)
                        {
                            del_user_funcs.ForEach(m =>
                            {
                                _context.UserFunctions.Remove(m);
                            });
                        }
                    }
                }

                if (add_ids != null && add_ids.Count > 0)
                {
                    add_ids.ForEach(m =>
                    {
                        _context.UserRoles.Add(new IdentityUserRole<int>()
                        {
                            UserId = request.UserId,
                            RoleId = m
                        });
                    });

                    var r_funcs = (from r in _context.Functions
                                   join g in _context.RoleFunctions on r.FunctionId equals g.FunctionId
                                   where add_ids.Contains(g.RoleId)
                                   select r.FunctionId).ToList();

                    if (r_funcs != null && r_funcs.Count > 0)
                    {
                        foreach (var f in r_funcs)
                        {
                            if (_context.UserFunctions.Find(request.UserId, f) == null)
                            {
                                _context.UserFunctions.Add(new UserFunction()
                                {
                                    UserId = request.UserId,
                                    FunctionId = f
                                });
                            }
                        }
                    }
                }

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<bool>(true);
                }

                return new ApiErrorResult<bool>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task<ApiResult<bool>> AssignFunction(UserAssignFunctionRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<bool>(ConstantHelper.AccountNotfound);
                }

                var selected_ids = request.Functions
                    .Where(m => m.Selected == true)
                    .Select(m => m.FunctionId)
                    .ToList();

                var curr_ids = _context.UserFunctions
                    .Where(m => m.UserId == request.UserId)
                    .Select(m => m.FunctionId)
                    .ToList();

                var remove_ids = curr_ids.Where(m => !selected_ids.Contains(m)).ToList();

                var add_ids = selected_ids.Where(m => !curr_ids.Contains(m)).ToList();

                if ((remove_ids == null || remove_ids.Count <= 0)
                    && (add_ids == null || add_ids.Count <= 0))
                {
                    return new ApiSuccessResult<bool>(true);
                }

                if (remove_ids != null && remove_ids.Count > 0)
                {
                    remove_ids.ForEach(m =>
                    {
                        _context.UserFunctions.Remove(new UserFunction()
                        {
                            UserId = request.UserId,
                            FunctionId = m
                        });
                    });
                }

                if (add_ids != null && add_ids.Count > 0)
                {
                    add_ids.ForEach(m =>
                    {
                        _context.UserFunctions.Add(new UserFunction()
                        {
                            UserId = request.UserId,
                            FunctionId = m
                        });
                    });
                }

                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    return new ApiSuccessResult<bool>(true);
                }

                return new ApiErrorResult<bool>(ConstantHelper.UpdateError);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        
    }
}
