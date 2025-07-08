using CMS.BaseModels.Common;
using CMS.Models.Authen.Genders;
using CMS.Models.Authen.Users;
using CMS.Models.Authen.UserStatuses;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CMS.WebApp.Controllers
{
    public class UserController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserStatusService _userStatusService;
        private readonly IGenderService _genderService;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(
            IConfiguration configuration,
            IUserStatusService userStatusService,
            IGenderService genderService,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IRoleService roleService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _userStatusService = userStatusService;
            _genderService = genderService;

            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _roleService = roleService;
        }

        public async Task AddUserStatusToViewBag()
        {
            try
            {
                var userStatuses = (await _userStatusService.GetAll()).ResultObj;
                userStatuses = userStatuses == null ? new List<UserStatusViewModel>() : userStatuses;
                ViewBag.UserStatuses = userStatuses;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task AddUserStatusSelectItemsToViewBag(byte selectedId)
        {
            try
            {
                var userStatuses = (await _userStatusService.GetAll()).ResultObj;
                userStatuses = userStatuses == null ? new List<UserStatusViewModel>() : userStatuses;
                var userStatus = new UserStatusViewModel()
                {
                    UserStatusId = 0,
                    UserStatusName = "Select status"
                };
                userStatuses.Insert(0, userStatus);
                ViewBag.UserStatuses = new SelectList(userStatuses, "UserStatusId", "UserStatusName", selectedId);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public async Task AddGenderSelectItemsToViewBag(byte selectedId)
        {
            try
            {
                var genders = (await _genderService.GetAll()).ResultObj;
                genders = genders == null ? new List<GenderViewModel>() : genders;
                var gender = new GenderViewModel()
                {
                    GenderId = 0,
                    GenderName = "Select gender"
                };
                genders.Insert(0, gender);
                ViewBag.Genders = new SelectList(genders, "GenderId", "GenderName", selectedId);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Index")]
        [HttpGet("user/index")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                await AddUserStatusToViewBag();

                ViewBag.Keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                var request = new GetUserPagingRequest()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Keyword = keyword
                };

                var result = await _userService.GetListPaging(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new PagedResult<UserViewModel>());
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Create")]
        [HttpGet("user/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddUserStatusSelectItemsToViewBag(1);
                await AddGenderSelectItemsToViewBag(0);

                return View();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Create")]
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateRequest request)
        {
            try
            {
                await AddUserStatusSelectItemsToViewBag(request.UserStatusId ?? 0);
                await AddGenderSelectItemsToViewBag((byte)(request.GenderId ?? 0));

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.Create(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.InsertSuccess;
                    return View(request);
                }

                ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Edit")]
        [HttpGet("user/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _userService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;

                    await AddUserStatusSelectItemsToViewBag(0);
                    await AddGenderSelectItemsToViewBag(0);

                    return View();
                }

                await AddUserStatusSelectItemsToViewBag(result.ResultObj.UserStatusId ?? 0);
                await AddGenderSelectItemsToViewBag((byte)(result.ResultObj.GenderId ?? 0));

                return View(new UserEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditRequest request)
        {
            try
            {
                await AddUserStatusSelectItemsToViewBag(request.UserStatusId ?? 0);
                await AddGenderSelectItemsToViewBag((byte)(request.GenderId ?? 0));

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.Update(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.UpdateSuccess;
                    return View(request);
                }

                ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Delete")]
        [HttpGet("user/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _userService.GetByIdToDelete(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(new UserDeleteRequest()
                {
                    Id = result.ResultObj.Id,
                    UserName = result.ResultObj.UserName,
                    FullName = result.ResultObj.FullName,
                    Deleteable = result.ResultObj.Deleteable
                });
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.Delete(request.Id);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.DeleteSuccess;
                }
                else
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;
                }

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-Details")]
        [HttpGet("user/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var result = await _userService.GetById(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-AssignRole")]
        [HttpGet("user/assignrole/{id}")]
        public async Task<IActionResult> AssignRole(int id)
        {
            try
            {
                var result = await _userService.GetUserRoles(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-AssignRole")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public async Task<IActionResult> AssignRole(UserAssignRoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.AssignRole(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.UpdateSuccess;
                    return View(request);
                }

                ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-AssignRole")]
        [HttpGet("user/assignfunction/{id}")]
        public async Task<IActionResult> AssignFunction(int id)
        {
            try
            {
                var result = await _userService.GetUserFunctions(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-AssignRole")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public async Task<IActionResult> AssignFunction(UserAssignFunctionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.AssignFunction(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.UpdateSuccess;
                    return View(request);
                }

                ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-UpdateProfile")]
        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                var result = await _userService.GetById(loginUserId);
                UpdateProfileRequest model = new UpdateProfileRequest(result.ResultObj);
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            };
        }

        //[Authorize(Roles = "User-UpdateProfile")]
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                if (request.Id != loginUserId)
                {
                    ViewBag.Error = "UserId is invalid";
                    return View(request);
                }

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.UpdateProfile(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.UpdateSuccess;
                    return View(request);
                }

                ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-ChangePassword")]
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                var result = await _userService.GetById(loginUserId);
                ChangePasswordRequest model = new ChangePasswordRequest(result.ResultObj);
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            };
        }

        //[Authorize(Roles = "User-ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                if (request.Id != loginUserId)
                {
                    ViewBag.Error = "UserId is invalid";
                    return View(request);
                }
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userService.ChangePassword(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.UpdateSuccess;
                    return View(request);
                }

                if (!string.IsNullOrEmpty(result.Message) && result.Message.Equals("Incorrect password."))
                {
                    ModelState.AddModelError("OldPassword", result.Message);
                }
                else ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "User-ChangeAvatar")]
        [HttpPost]
        public async Task<JsonResult> ChangeAvatar(IFormFile avatar)
        {
            ApiResult<string> retval = new ApiResult<string>();
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                var user = await _userService.GetById(loginUserId);
                if (!user.IsSuccessed || user.ResultObj == null || user.ResultObj.Id <= 0)
                {
                    retval = new ApiErrorResult<string>(ConstantHelper.AccountNotfound);
                    return Json(retval);
                }
                if (avatar != null)
                {
                    var originalFileName = ContentDispositionHeaderValue.Parse(avatar.ContentDisposition).FileName.Trim('"');
                    var fileName = $"{StringHelper.RemoveSignatureForURL(Path.GetFileNameWithoutExtension(originalFileName))}-{DateTime.Now.ToString("HHmmss")}{Path.GetExtension(originalFileName)}";


                    var filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\" + ConstantHelper.AvatarPath;
                    string subPath = $"{DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "\\")}";

                    if (!Directory.Exists(Path.Combine(filePath, subPath)))
                    {
                        Directory.CreateDirectory(Path.Combine(filePath, subPath));
                    }

                    fileName = Path.Combine(subPath, fileName);

                    using var output = new FileStream(Path.Combine(filePath, fileName), FileMode.Create);
                    avatar.OpenReadStream().CopyTo(output);
                    output.Close();

                    var request = new ChangeAvatarRequest()
                    {
                        Id = loginUserId,
                        Avatar = fileName
                    };
                    var result = await _userService.ChangeAvatar(request);

                    if (result.IsSuccessed)
                    {
                        retval = new ApiSuccessResult<string>(ConstantHelper.AvatarPath + fileName.Replace("\\", "/"));
                    }
                    else
                    {
                        retval = new ApiErrorResult<string>(result.ResultObj);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                retval = new ApiErrorResult<string>(ex.ToString());
                throw;
            }
            return Json(retval);
        }

        //[Authorize(Roles = "User-ChangeCoverPhoto")]
        [HttpPost]
        public async Task<JsonResult> ChangeCoverPhoto(IFormFile cover)
        {
            ApiResult<string> retval = new ApiResult<string>();
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                var user = await _userService.GetById(loginUserId);
                if (!user.IsSuccessed || user.ResultObj == null || user.ResultObj.Id <= 0)
                {
                    retval = new ApiErrorResult<string>(ConstantHelper.AccountNotfound);
                    return Json(retval);
                }
                if (cover != null)
                {
                    var originalFileName = ContentDispositionHeaderValue.Parse(cover.ContentDisposition).FileName.Trim('"');
                    var fileName = $"{StringHelper.RemoveSignatureForURL(Path.GetFileNameWithoutExtension(originalFileName))}-{DateTime.Now.ToString("HHmmss")}{Path.GetExtension(originalFileName)}";


                    var filePath = Directory.GetCurrentDirectory() + "\\wwwroot\\" + ConstantHelper.CoverPath;
                    string subPath = $"{DateTime.Now.ToString("yyyy/MM/dd").Replace("/", "\\")}";

                    if (!Directory.Exists(Path.Combine(filePath, subPath)))
                    {
                        Directory.CreateDirectory(Path.Combine(filePath, subPath));
                    }

                    fileName = Path.Combine(subPath, fileName);

                    using var output = new FileStream(Path.Combine(filePath, fileName), FileMode.Create);
                    cover.OpenReadStream().CopyTo(output);
                    output.Close();

                    var request = new ChangeCoverPhotoRequest()
                    {
                        Id = loginUserId,
                        CoverPhoto = fileName
                    };
                    var result = await _userService.ChangeCoverPhoto(request);

                    if (result.IsSuccessed)
                    {
                        retval = new ApiSuccessResult<string>(ConstantHelper.CoverPath + fileName.Replace("\\", "/"));
                    }
                    else
                    {
                        retval = new ApiErrorResult<string>(result.ResultObj);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                retval = new ApiErrorResult<string>(ex.ToString());
                throw;
            }
            return Json(retval);
        }

        //[Authorize(Roles = "User-ChangePassword")]
        [HttpGet("user/resetpassword/{userId}")]
        public async Task<IActionResult> ResetPassword(int userId)
        {
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                var result = await _userService.GetById(userId);
                ResetPasswordRequest model = new ResetPasswordRequest(result.ResultObj);
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            };
        }

        //[Authorize(Roles = "User-ChangePassword")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var loginUserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var roleResult = await _roleService.GetListByUser(loginUserId);

                if(roleResult == null || !roleResult.IsSuccessed
                    || roleResult.ResultObj == null || roleResult.ResultObj.Count <= 0)
                {
                    ViewBag.Error = "Bạn không có quyền thực hiện chức năng này";
                    return View(request);
                }

                var adminRole = false;
                var shopManagerRole = false;
                var roleList = roleResult.ResultObj;
                foreach(var role in roleList)
                {
                    if(role.Id == ConstantHelper.SysAdmin_RoleId)
                    {
                        adminRole = true;
                    }
                    if (role.Id == ConstantHelper.ShopManager_RoleId)
                    {
                        shopManagerRole = true;
                    }
                }

                if(!adminRole && !shopManagerRole)
                {
                    ViewBag.Error = "Bạn không có quyền thực hiện chức năng này";
                    return View(request);
                }

                var result = await _userService.ResetPassword(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = "Thay đổi mật khẩu thành công";
                    return View(request);
                }

                ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
    }
}
