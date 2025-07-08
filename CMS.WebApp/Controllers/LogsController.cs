using CMS.BaseModels.Common;
using CMS.Models.Authen.Icons;
using CMS.Models.Authen.UserLogs;
using CMS.Models.Authen.Users;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.WebApp.Controllers
{
    public class LogsController : BaseController

    {
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUserService _userService;
        private readonly IUserLogService _userLogService;
        public LogsController(IFunctionService functionService,
                              IHttpContextAccessor httpContextAccessor,
                              IUserService userService,
                              IUserLogService userLogService) : base(functionService,
                                                                     httpContextAccessor,
                                                                     userService)
        {
            _functionService = functionService;
            _contextAccessor = httpContextAccessor;
            _userService = userService;
            _userLogService = userLogService;
        }
        public async Task AddUserstoViewBag()
        {
            try
            {
                var users = (await _userService.GetAll()).ResultObj;
                users = users == null ? new List<UserViewModel>() : users;
                var defaultSelect = new UserViewModel()
                {
                    Id = 0,
                    FullName = "",
                };
                users.Insert(0, defaultSelect);
                ViewBag.UserList = users;
                ViewBag.Users = new SelectList(users, "Id", "FullName", 0);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        [HttpGet("logs")]
        public async Task<IActionResult> Index(string keyword,
                                               int? ActionId,
                                               int? StatusId,
                                               int pageIndex = 1,
                                               int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                await AddUserstoViewBag();
                string key = !string.IsNullOrWhiteSpace(keyword) ? keyword : "";
                ViewBag.Keyword = key;
                var request = new GetUserLogPagingRequest()
                {
                    Keyword = key,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    ActionId = ActionId != null ? ActionId : null,
                    StatusId = StatusId != null ? StatusId : null
                };
                var result = await _userLogService.GetListPaging(request);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new PagedResult<UserLogViewModel>());
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod()!.Name);
                throw;
            }
        }
    }
}
