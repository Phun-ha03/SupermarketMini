using CMS.Models.Authen.Functions;
using CMS.Models.Supermarket.Categories;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApp.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        private readonly ICategoryService _categoryService;

        public CategoryController(IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            ICategoryService CategoryService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _categoryService = CategoryService;
        }

        [HttpGet("Category/index")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                ViewBag.Keyword = keyword;

                var request = new GetCategoryPagingRequest()
                {
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                var result = await _categoryService.GetListPaging(request);

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

        [HttpGet("Category/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _categoryService.Create(request);

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

        [HttpGet("Category/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _categoryService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new CategoryEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _categoryService.Update(request);

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

        [HttpGet("Category/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _categoryService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new CategoryDeleteRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _categoryService.Delete(request.CategoryID);

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
        [HttpGet]
        public async Task<IActionResult> ExportToExcel()
        {
            var content = await _categoryService.ExportCategoriesToExcelAsync();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachDanhMucHang.xlsx");
        }
    }
}
