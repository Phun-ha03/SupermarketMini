using CMS.BaseModels.Common;
using CMS.Models.Authen.Icons;
using CMS.Models.Authen.Roles;
using CMS.Services.Authen.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.WebApp.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;
        private readonly IIconService _iconService;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public RoleController(IConfiguration configuration,
            IRoleService roleService,
            IIconService iconService,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _roleService = roleService;
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
        public async Task AddParentRolesToViewBag(int parentRoleId)
        {
            try
            {
                var roles = (await _roleService.GetHỉerachy(new GetRoleHierarchyRequest())).ResultObj;
                roles = roles == null ? new List<RoleViewModel>() : roles;
                var role = new RoleViewModel()
                {
                    Id = 0,
                    Description = "Chọn chức năng cha"
                };
                roles.Insert(0, role);
                ViewBag.Roles = new SelectList(roles, "Id", "Description", parentRoleId);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "Role-Index")]
        [HttpGet("role/index")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                ViewBag.Keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                var request = new GetRolePagingRequest()
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Keyword = keyword
                };

                var result = await _roleService.GetListPaging(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new PagedResult<RoleViewModel>());
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
            }
            return View();
        }

        //[Authorize(Roles = "Role-Create")]
        [HttpGet("role/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddIconsToViewBag();
                await AddParentRolesToViewBag(0);

                return View();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "Role-Create")]
        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateRequest request)
        {
            try
            {
                await AddIconsToViewBag();

                if (!ModelState.IsValid)
                {
                    await AddParentRolesToViewBag(request.ParentRoleId);
                    return View(request);
                }

                request.IsShow = (byte)(request.IsShowBoolean ? 1 : 0);
                var result = await _roleService.Create(request);

                await AddParentRolesToViewBag(request.ParentRoleId);

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

        //[Authorize(Roles = "Role-Edit")]
        [HttpGet("role/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _roleService.GetById(id);

                await AddIconsToViewBag();
                await AddParentRolesToViewBag(result.ResultObj == null ? 0 : result.ResultObj.ParentRoleId);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new RoleEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        //[Authorize(Roles = "Role-Edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditRequest request)
        {
            try
            {
                await AddIconsToViewBag();

                if (!ModelState.IsValid)
                {
                    await AddParentRolesToViewBag(request.ParentRoleId);
                    return View(request);
                }

                request.IsShow = (byte)(request.IsShowBoolean ? 1 : 0);
                var result = await _roleService.Update(request);

                await AddParentRolesToViewBag(request.ParentRoleId);

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

        //[Authorize(Roles = "Role-Delete")]
        [HttpGet("role/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _roleService.GetById(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }
                return View(new RoleDeleteRequest()
                {
                    Id = result.ResultObj.Id,
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

        //[Authorize(Roles = "Role-Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(RoleDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _roleService.Delete(request.Id);

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

        //[Authorize(Roles = "Role-Details")]
        [HttpGet("role/details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var result = await _roleService.GetWithClaimsById(id);
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

        [HttpGet("role/assignfunction/{id}")]
        public async Task<IActionResult> AssignFunction(int id)
        {
            try
            {
                var result = await _roleService.GetWithFunctionsById(id);
                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new RoleAssignFunctionRequest()
                {
                    RoleId = result.ResultObj.Id,
                    RoleName = result.ResultObj.Name,
                    Functions = result.ResultObj.Functions
                });
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueCountLimit = int.MaxValue)]
        public async Task<IActionResult> AssignFunction(RoleAssignFunctionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Error = "Model invalid";
                    return View(request);
                }

                var result = await _roleService.AssignFunction(request);

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
            }
            return View();
        }
    }
}

