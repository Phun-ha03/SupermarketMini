using CMS.Models.Authen.Functions;
using CMS.Models.Authen.Icons;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.WebApp.Controllers
{
    public class FunctionController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IIconService _iconService;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public FunctionController(IConfiguration configuration,
            IFunctionService functionService,
            IIconService iconService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
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
        public async Task AddParentFunctionsToViewBag(int parentFunctionId)
        {
            try
            {
                var functions = (await _functionService.GetHỉerachy(new GetFunctionHierarchyRequest())).ResultObj;
                functions = functions == null ? new List<FunctionViewModel>() : functions;
                var function = new FunctionViewModel()
                {
                    FunctionId = 0,
                    Description = "Chọn chức năng cha"
                };
                functions.Insert(0, function);
                ViewBag.Functions = new SelectList(functions, "FunctionId", "Description", parentFunctionId);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpGet("function/index")]
        public async Task<IActionResult> Index(string keyword, int parentId, byte isShow = 2, byte statusId = 2)
        {
            try
            {
                keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                ViewBag.Keyword = keyword;

                await AddParentFunctionsToViewBag(parentId);

                var request = new GetFunctionHierarchyRequest()
                {
                    ParentFunctionId = parentId,
                    UserId = 0,
                    RoleId = 0,
                    Keyword = keyword,
                    IsShow = isShow,
                    StatusId = statusId
                };

                var result = await _functionService.GetHỉerachy(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new List<FunctionViewModel>());
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpGet("function/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddIconsToViewBag();
                await AddParentFunctionsToViewBag(0);

                return View();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(FunctionCreateRequest request)
        {
            try
            {
                await AddIconsToViewBag();

                if (!ModelState.IsValid)
                {
                    await AddParentFunctionsToViewBag(request.ParentFunctionId);
                    return View(request);
                }

                request.IsShow = (byte)(request.IsShowBoolean ? 1 : 0);
                var result = await _functionService.Create(request);

                await AddParentFunctionsToViewBag(request.ParentFunctionId);

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

        [HttpGet("function/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _functionService.GetById(id);

                await AddIconsToViewBag();
                await AddParentFunctionsToViewBag(result.ResultObj == null ? 0 : result.ResultObj.ParentFunctionId);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new FunctionEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FunctionEditRequest request)
        {
            try
            {
                await AddIconsToViewBag();

                if (!ModelState.IsValid)
                {
                    await AddParentFunctionsToViewBag(request.ParentFunctionId);
                    return View(request);
                }

                request.IsShow = (byte)(request.IsShowBoolean ? 1 : 0);
                var result = await _functionService.Update(request);

                await AddParentFunctionsToViewBag(request.ParentFunctionId);

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

        [HttpGet("function/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _functionService.GetById(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(new FunctionDeleteRequest()
                {
                    FunctionId = result.ResultObj.FunctionId,
                    Name = result.ResultObj.Name,
                    Description = result.ResultObj.Description
                });
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(FunctionDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _functionService.Delete(request.FunctionId);

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
