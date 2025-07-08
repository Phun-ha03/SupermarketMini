using CMS.BaseModels.Common;
using CMS.Models.Authen.Icons;
using CMS.Models.Authen.UserStatuses;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApp.Controllers
{
    public class UserStatusController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IUserStatusService _userStatusService;
        private readonly IIconService _iconService;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public UserStatusController(
            IConfiguration configuration,
            IUserStatusService UserStatusService,
            IIconService iconService,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _userStatusService = UserStatusService;
            _iconService = iconService;

            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;

        }

        public async Task AddIconsToViewBag()
        {
            try
            {
                var icons = (await _iconService.GetAllActive()).ResultObj;
                icons = icons == null ? new List<IconViewModel>() : icons;
                ViewBag.Icons = icons;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "UserStatus-Index")]
        [HttpGet("UserStatus/index")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                ViewBag.Keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                var request = new GetUserStatusPagingRequest()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Keyword = keyword
                };

                var result = await _userStatusService.GetListPaging(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new PagedResult<UserStatusViewModel>());
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "UserStatus-Create")]
        [HttpGet("UserStatus/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddIconsToViewBag();

                //ViewBag.Colors = (await _colorService.GetHỉerachy(new GetRoleHierarchyRequest())).ResultObj;

                var userStatus = await _userStatusService.GetByIdMax();
                var userStatusCreateRequest = new UserStatusCreateRequest()
                {
                    UserStatusId = (byte)(userStatus.ResultObj.UserStatusId + 1),
                    UserStatusName = string.Empty,
                    UserStatusDesc = string.Empty,
                    Color = string.Empty,
                    BackgroundColor = string.Empty
                };

                return View(userStatusCreateRequest);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "UserStatus-Create")]
        [HttpPost]
        public async Task<IActionResult> Create(UserStatusCreateRequest request)
        {
            try
            {
                await AddIconsToViewBag();

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userStatusService.Create(request);

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

        //[Authorize(Roles = "UserStatus-Edit")]
        [HttpGet("UserStatus/edit/{id}")]
        public async Task<IActionResult> Edit(byte id)
        {
            try
            {
                await AddIconsToViewBag();

                var result = await _userStatusService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;

                    return View();
                }

                return View(new UserStatusEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "UserStatus-Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(UserStatusEditRequest request)
        {
            try
            {
                await AddIconsToViewBag();

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userStatusService.Update(request);

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

        //[Authorize(Roles = "UserStatus-Delete")]
        [HttpGet("UserStatus/delete/{id}")]
        public async Task<IActionResult> Delete(byte id)
        {
            try
            {
                var result = await _userStatusService.GetById(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(new UserStatusDeleteRequest()
                {
                    UserStatusId = result.ResultObj.UserStatusId,
                    UserStatusName = result.ResultObj.UserStatusName,
                    UserStatusDesc = result.ResultObj.UserStatusDesc,
                    Color = result.ResultObj.Color,
                    BackgroundColor = result.ResultObj.BackgroundColor,
                });
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "UserStatus-Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(UserStatusDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _userStatusService.Delete(request.UserStatusId);

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

        //[Authorize(Roles = "UserStatus-Details")]
        [HttpGet("UserStatus/details/{id}")]
        public async Task<IActionResult> Details(byte id)
        {
            try
            {
                var result = await _userStatusService.GetById(id);
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
    }
}
