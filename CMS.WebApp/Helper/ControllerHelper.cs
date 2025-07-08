using CMS.Models.Authen.Functions;
using CMS.Models.Authen.Users;
using CMS.Services.Authen.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CMS.WebApp.Helper
{
    public class ControllerHelper
    {
        public static void addUserInfoToViewBag(Controller control
            , IFunctionService _functionService
            , IHttpContextAccessor _httpContextAccessor
            , IUserService _userService
            , int loginUserId)
        {
            var context = control.ControllerContext;

            var action = context.ActionDescriptor.ActionName;
            var controller = context.ActionDescriptor.ControllerName;

            var menu_json = string.Empty;
            var user_json = string.Empty;
            var user = new UserViewModel();
            var menus = new List<FunctionViewModel>();

            try
            {
                menu_json = _httpContextAccessor.HttpContext?.Session.GetString("Menu");
                user_json = _httpContextAccessor.HttpContext?.Session.GetString("UserData");

                /*if (string.IsNullOrEmpty(user_json)) {
                    control.Request.Cookies.TryGetValue("UserData", out user_json);
                }
                if (string.IsNullOrEmpty(menu_json))
                {
                    control.Request.Cookies.TryGetValue("Menu", out menu_json);
                }*/
            }
            catch (Exception ex) { }

            if (string.IsNullOrEmpty(user_json))
            {
                try
                {
                    var apiResult = _userService.GetById(loginUserId).Result;
                    if (apiResult.IsSuccessed && apiResult.ResultObj != null && apiResult.ResultObj.Id > 0)
                    {
                        user = apiResult.ResultObj;
                    }
                }
                catch (Exception ex)
                {
                }
                if (user != null && user.Id > 0)
                {
                    try
                    {
                        _httpContextAccessor.HttpContext?.Session.SetString("UserData", JsonConvert.SerializeObject(user));
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                user = JsonConvert.DeserializeObject<UserViewModel>(user_json);
            }

            if (user != null && user.Id > 0)
            {
                control.ViewBag.User = user;
            }

            if (string.IsNullOrEmpty(menu_json))
            {
                try
                {
                    var apiResult = _functionService.GetAllActiveUserMenus(new GetUserMenuRequest()
                    {
                        UserId = loginUserId
                    }).Result;
                    if (apiResult.IsSuccessed && apiResult.ResultObj != null && apiResult.ResultObj.Count > 0)
                    {
                        menus = apiResult.ResultObj;
                    }
                }
                catch (Exception ex)
                {
                }

                if (menus != null && menus.Count > 0)
                {
                    try
                    {
                        _httpContextAccessor.HttpContext?.Session.SetString("Menu", JsonConvert.SerializeObject(menus));
                    }
                    catch (Exception ex) { }
                }
            }
            else
            {
                menus = JsonConvert.DeserializeObject<List<FunctionViewModel>>(menu_json);
            }

            if (menus != null && menus.Count > 0)
            {
                menus.ForEach(m => { m.Selected = false; });

                var curr_menu = menus.Where(m => m.Controler.Equals(controller) && m.Action.Equals(action))
                    .FirstOrDefault();
                if (curr_menu != null)
                {
                    try
                    {
                        _httpContextAccessor.HttpContext?.Session.SetString("curr_menu", JsonConvert.SerializeObject(curr_menu));
                    }
                    catch (Exception ex) { }
                }
                else if(!action.ToLower().Equals("profitmustpay"))
                {
                    var curr_menu_json = string.Empty;
                    try
                    {
                        curr_menu_json = _httpContextAccessor.HttpContext?.Session.GetString("curr_menu");
                    }
                    catch (Exception ex)
                    {
                    }

                    if (!string.IsNullOrEmpty(curr_menu_json))
                    {
                        curr_menu = JsonConvert.DeserializeObject<FunctionViewModel>(curr_menu_json);
                    }
                }
                if (curr_menu != null)
                {
                    menus.Where(m => m.FunctionId == curr_menu.FunctionId).ToList().ForEach(m => { m.Selected = true; });
                    while (curr_menu.ParentFunctionId > 0)
                    {
                        curr_menu = menus.Where(m => m.FunctionId == curr_menu.ParentFunctionId)
                            .FirstOrDefault();
                        if (curr_menu == null) break;
                        curr_menu.Selected = true;
                    }
                }

                control.ViewBag.Menu = menus;
            }
        }
    }
}
