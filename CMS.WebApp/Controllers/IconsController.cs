using Microsoft.AspNetCore.Mvc;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using CMS.Models.Authen.Icons;
using CMS.BaseModels.Common;
using CMS.Models.Authen.UserStatuses;
namespace CMS.WebApp.Controllers
{
    public class IconsController : BaseController
    {
        //private readonly iCMSDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserStatusService _userStatusService;
        private readonly IGenderService _genderService;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IIconService _iconService;

        public IconsController(
            IConfiguration configuration,
            IUserStatusService userStatusService,
            IGenderService genderService,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IRoleService roleService,
            IIconService iconService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _userStatusService = userStatusService;
            _genderService = genderService;

            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _roleService = roleService;
            _iconService = iconService;
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
        // GET: Icons
        [HttpGet("icons/index")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                ViewBag.Keyword = keyword;

                await AddUserStatusToViewBag();

                var request = new GetIconPagingRequest()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Keyword = keyword
                };

                var result = await _iconService.GetListPaging(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new PagedResult<IconViewModel>());
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        //Create
        [HttpGet("icons/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddUserStatusToViewBag();
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(IconCreateRequest request)
        {
            try
            {
                await AddUserStatusToViewBag();

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _iconService.Create(request);
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

        //Edit
        [HttpGet("icons/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _iconService.GetById(id);

                await AddUserStatusToViewBag();
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(new IconEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IconEditRequest request)
        {
            try
            {
                await AddUserStatusToViewBag();
                if (!ModelState.IsValid)
                {

                    return View(request);
                }
                var result = await _iconService.Update(request);
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
        [HttpGet("icons/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _iconService.GetById(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(new IconDeleteRequest()
                {
                    IconId = result.ResultObj.IconId,
                    IconCode = result.ResultObj.IconCode,
                    IconTypeId = result.ResultObj.IconTypeId,
                    IconTypeCode = result.ResultObj.IconTypeCode,
                });
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "Role-Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(IconDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                var result = await _iconService.Delete(request.IconId);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.DeleteSuccess;
                    //return RedirectToAction("index");
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

    }
}
