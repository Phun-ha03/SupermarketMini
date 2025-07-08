using CMS.Models.Authen.Users;
using CMS.Services.Authen.Interfaces;
using CMS.WebApp.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMS.WebApp.Controllers
{
    public class BaseController : Controller
    {
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public BaseController(IFunctionService functionService
            , IHttpContextAccessor httpContextAccessor
            , IUserService userService
        )
        {
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public override async void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            //base.OnActionExecuting(context);
            var userDataJson = "";
            UserViewModel userViewModel = new UserViewModel();
            var loginUserId = UserHelper.getLoginUserId(User);
            if (loginUserId <= 0)
            {
                /*Request.Cookies.TryGetValue("UserData", out userDataJson);
                if (!string.IsNullOrEmpty(userDataJson)) {
                    userViewModel = JsonConvert.DeserializeObject<UserViewModel>(userDataJson);
                    loginUserId = userViewModel.Id;
                }*/
                if (loginUserId <= 0)
                {
                    context.Result = new RedirectToActionResult("login", "account", null);
                }
            }

            var action = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var controller = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerName;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true);
                var controllerAttributes = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(true);
                bool isAnonymous = actionAttributes.OfType<AllowAnonymousAttribute>().Any() ||
                              controllerAttributes.OfType<AllowAnonymousAttribute>().Any();
                if (!isAnonymous)
                {
                    var checkPermission = _functionService.CheckPermission(loginUserId, controller.ToLower(), action.ToLower()).Result;
                    if (checkPermission == null || !checkPermission.ResultObj)
                    {
                        context.Result = new RedirectToActionResult("accessdenied", "account", null);
                    }
                }
            }
            /*var actionLower = action.ToLower();
            if (!(new List<string>() { "account", "camsim", "function", "role", "rolegroup", "shop", "user", "userstatus" }).Contains(actionLower)) {
                Response.Redirect("https://jobman.icsoft.vn/");
                return;
            }*/

            ControllerHelper.addUserInfoToViewBag(this, _functionService, _httpContextAccessor, _userService, loginUserId);
        }
    }
}
