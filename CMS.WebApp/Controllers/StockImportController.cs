using CMS.BaseModels.Common;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.StockImportDetails;
using CMS.Models.Supermarket.StockImports;
using CMS.Models.Supermarket.Suppliers;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.WebApp.Controllers
{
    public class StockImportController : BaseController
    {
        private readonly ILogger<StockImportController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IStockImportService _stockImportService;
        private readonly IStockImportDetailService _stockImportDetailService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;

        public StockImportController(
            ILogger<StockImportController> logger,
            IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IStockImportService stockImportService,
            IStockImportDetailService stockImportDetailService,
            ISupplierService supplierService,
            IProductService productService) : base(functionService, httpContextAccessor, userService)
        {
            _logger = logger;
            _configuration = configuration;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _stockImportService = stockImportService;
            _supplierService = supplierService;
            _productService = productService;
            _stockImportDetailService= stockImportDetailService;
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
        [HttpGet]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                keyword = keyword ?? string.Empty;
                ViewBag.Keyword = keyword;

                var request = new GetStockImportPagingRequest()
                {
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                };

                var result = await _stockImportService.GetListPaging(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new PagedResult<StockImportViewModel>());
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        [HttpGet("StockImport/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddSupplierSelectItemsToViewBag(0);

                var model = new StockImportCreateRequest
                {
                    ImportDate = DateTime.Now,
                    DiscountAmount = 0
                };

                return View(model); 
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(StockImportCreateRequest request)
        {
            try
            {
                await AddSupplierSelectItemsToViewBag(request.SupplierID);

                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _stockImportService.Create(request);

                if (result.IsSuccessed)
                {
                    ViewBag.Message = ConstantHelper.InsertSuccess;
                    return RedirectToAction("Index");
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
        [HttpGet("StockImport/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _stockImportService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new StockImportDeleteRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(StockImportDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _stockImportService.Delete(request.StockImportID);

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
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _stockImportService.GetById(id);
            if (!result.IsSuccessed || result.ResultObj == null)
            {
                TempData["Error"] = result.Message ?? "Không tìm thấy phiếu nhập.";
                return RedirectToAction("Index");
            }
            var stockImport = result.ResultObj;

            var details = await _stockImportDetailService.GetDetailsByImportId(id);
            stockImport.StockImportDetails = details ?? new List<StockImportDetailViewModel>();

            return View(stockImport); // View model là StockImportViewModel
        }


        [HttpGet]
        public async Task<JsonResult> GetProductsByKeywordAjax(string keyword)
        {
            try
            {
                var result = await _productService.GetProductsByKeyword(keyword);
                if (result == null || !result.IsSuccessed)
                {
                    return Json(new { isSuccessed = false, message = result?.Message ?? "Không tìm thấy sản phẩm" });
                }

                return Json(new { isSuccessed = true, data = result.ResultObj });
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần
                return Json(new { isSuccessed = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetUnitsByProductID(int productID)
        {
            var units = await _productService.GetUnitsByProductID(productID); // Hàm lấy danh sách đơn vị theo productID
            return Json(new { data = units });
        }

    }
}