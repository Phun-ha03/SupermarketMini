using Azure.Core;
using CMS.BaseModels.Common;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Authen.Functions;
using CMS.Models.Authen.Genders;
using CMS.Models.Authen.Icons;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.StockImportDetails;
using CMS.Models.Supermarket.Suppliers;
using CMS.Services.Authen;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using DocumentFormat.OpenXml.Office2016.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.WebApp.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        private readonly IProductService _productService;
        private readonly IProductUnitService _productUnitService;
        private readonly ISupplierService _supplierService;
        private readonly ICategoryService _categoryService;

        public ProductController(
            IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IProductService productService,
            IProductUnitService productUnitService,
            ISupplierService supplierService,
            ICategoryService categoryService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _productService = productService;
            _productUnitService = productUnitService;
            _supplierService = supplierService;
            _categoryService = categoryService;
        }


        public async Task AddSupplierSelectItemsToViewBag(int selectedId)
        {
            try
            {
                var suppliers = (await _supplierService.GetAll()).ResultObj;
                suppliers = suppliers ?? new List<SupplierViewModel>();

                var defaultSupplier = new SupplierViewModel()
                {
                    SupplierID = 0,
                    SupplierName = "Chọn nhà cung cấp"
                };

                suppliers.Insert(0, defaultSupplier);

                ViewBag.Suppliers = new SelectList(suppliers, "SupplierID", "SupplierName", selectedId);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        public async Task AddCategorySelectItemsToViewBag(int selectedId = 0)
        {
            try
            {
                var categories = (await _categoryService.GetAll()).ResultObj;
                categories = categories ?? new List<CategoryViewModel>();

                var defaultCategory = new CategoryViewModel()
                {
                    CategoryID = 0,
                    CategoryName = "Chọn danh mục"
                };

                categories.Insert(0, defaultCategory);

                ViewBag.Categories = new SelectList(categories, "CategoryID", "CategoryName", selectedId);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }



        //[HttpGet("Product/index")]
        //public async Task<IActionResult> Index(string keyword, string priceFilter, int? categoryID, int? supplierID, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        //{

        //    try
        //    {
        //        keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
        //        priceFilter = string.IsNullOrEmpty(priceFilter) ? string.Empty : priceFilter;
        //        ViewBag.Keyword = keyword;

        //        await AddCategorySelectItemsToViewBag(0);
        //        await AddSupplierSelectItemsToViewBag(0);

        //        ViewBag.PriceFilter = priceFilter;
        //        var request = new GetProductPagingRequest()
        //        {
        //            Keyword = keyword,
        //            PriceFilter = priceFilter,
        //            CategoryID = categoryID,
        //            SupplierID = supplierID,
        //            PageIndex = pageIndex,
        //            PageSize = pageSize,
        //        };

        //        var result = await _productService.GetListPaging(request);

        //        if (!result.IsSuccessed)
        //        {
        //            ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
        //            return View(new List<FunctionViewModel>());
        //        }


        //        return View(result.ResultObj);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
        //        throw;
        //    }
        //}
        [HttpGet("Product/index")]
        public async Task<IActionResult> Index(string keyword, string priceFilter, int? categoryID, int? supplierID, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                // Nếu null thì gán string.Empty
                keyword = keyword ?? string.Empty;
                priceFilter = priceFilter ?? string.Empty;

                ViewBag.Keyword = keyword;
                ViewBag.PriceFilter = priceFilter;
                ViewBag.CategoryID = categoryID ?? 0;
                ViewBag.SupplierID = supplierID ?? 0;

                await AddCategorySelectItemsToViewBag(ViewBag.CategoryID);
                await AddSupplierSelectItemsToViewBag(ViewBag.SupplierID);

                // Tạo request để gọi API
                var request = new GetProductPagingRequest()
                {
                    Keyword = keyword,
                    priceFilter = priceFilter,
                    CategoryID = categoryID,
                    SupplierID = supplierID,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                };

                var result = await _productService.GetListPaging(request);

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

        [HttpGet("Product/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddCategorySelectItemsToViewBag(0);
                await AddSupplierSelectItemsToViewBag(0);
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateRequest request)
        {
            try
            {
                await AddSupplierSelectItemsToViewBag((byte)(request.SupplierID));
                await AddCategorySelectItemsToViewBag((byte)(request.CategoryID));
                if (!ModelState.IsValid)
                {
                    return View(request);
                }
                
                var result = await _productService.Create(request);

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

        [HttpGet("Product/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _productService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;

                    await AddCategorySelectItemsToViewBag(0);
                    await AddSupplierSelectItemsToViewBag(0);

                    return View();
                }

                await AddSupplierSelectItemsToViewBag((result.ResultObj.SupplierID ?? 0));
                await AddCategorySelectItemsToViewBag((result.ResultObj.CategoryID ?? 0));

                return View(new ProductEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditRequest request)
        {
            try
            {

                await AddCategorySelectItemsToViewBag(request.CategoryID ?? 0);
                await AddSupplierSelectItemsToViewBag(request.SupplierID ?? 0);
                if (!ModelState.IsValid)
                {

                    return View(request);
                }

                var result = await _productService.Update(request);

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

        [HttpGet("Product/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _productService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new ProductDeleteRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _productService.Delete(request.ProductID);

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
        public async Task<IActionResult> ExportCsv()
        {
            var fileBytes = await _productService.ExportCsv();
            var fileName = $"SanPham_{DateTime.Now:yyyyMMddHHmmss}.csv";
            return File(fileBytes, "text/csv", fileName);
        }
        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            var fileBytes = await _productService.ExportProductsToExcelAsync();
            var fileName = $"DanhSachSanPham_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }


    }
}
