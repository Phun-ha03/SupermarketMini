using CMS.Models.Authen.Functions;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.OrderDetails;
using CMS.Models.Supermarket.Orders;
using CMS.Models.Supermarket.Reports;
using CMS.Models.Supermarket.StockImportDetails;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApp.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IProductService _productService;
        private readonly IProductUnitService _productUnitService;
        private readonly ICustomerService _customerService;

        public OrderController(IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IProductService productService,
            IProductUnitService productUnitService,
            IOrderService orderService,
            ICustomerService customerService,
            IOrderDetailService orderDetailService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _orderService = orderService;
            _productService = productService;
            _productUnitService = productUnitService;
            _orderDetailService = orderDetailService;
            _customerService = customerService;
        }

        [HttpGet("Order/index")]
        public async Task<IActionResult> Index(string keyword, DateTime? searchDate, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                ViewBag.Keyword = keyword;
                ViewBag.SearchDate = searchDate;
                var request = new GetOrderPagingRequest()
                {
                    Keyword = keyword,
                    SearchDate = searchDate,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                var result = await _orderService.GetListPaging(request);

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


       
        [HttpGet("Order/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _orderService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new OrderDeleteRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        [HttpGet("Order/create")]
        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetAll();
            var units = await _productUnitService.GetAll();

            ViewBag.Products = products.ResultObj;
            ViewBag.Units = units.ResultObj;

            var model = new OrderCreateRequest
            {
                OrderDetails = new List<OrderDetailCreateRequest>
        {
            new OrderDetailCreateRequest() // ít nhất một dòng mặc định
        }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                // Lấy ID người dùng hiện tại từ claim
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdString, out var currentUserId))
                {
                    ViewBag.Error = "Không xác định được tài khoản hiện tại.";
                    return View(request);
                }

                // Gọi service và truyền currentUserId
                var result = await _orderService.Create(request, currentUserId);

                if (result.IsSuccessed)
                {
                    int orderId = result.ResultObj.OrderID;
                    return RedirectToAction("Detail", new { id = orderId });
                }

                ViewBag.Error = result.Message ?? ConstantHelper.UpdateError;
                return View(request);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), nameof(Create));
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(OrderDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _orderService.Delete(request.OrderID);

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

        [HttpGet]
        public async Task<JsonResult> GetCustomerByPhoneAjax(string phone)
        {
            var result = await _customerService.GetCustomerByPhone(phone);
            if (result.IsSuccessed)
            {
                return Json(new
                {
                    success = true,
                    data = new
                    {
                        id = result.ResultObj.CustomerID,
                        name = result.ResultObj.Name,
                        phone = result.ResultObj.Phone,
                        loyaltyPoints = result.ResultObj.LoyalPoints
                    }
                });
            }

            return Json(new { success = false, message = result.Message });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var result = await _orderService.GetById(id);
            if (!result.IsSuccessed || result.ResultObj == null)
            {
                TempData["Error"] = result.Message ?? "Không tìm thấy phiếu nhập.";
                return RedirectToAction("Index");
            }
            var order = result.ResultObj;

            var details = await _orderDetailService.GetDetailsByOrderId(id);
            order.OrderDetails = details ?? new List<OrderDetailViewModel>();

            return View(order); 
        }

        public IActionResult RevenueByDate(DateTime? from, DateTime? to)
        {
            var fromDate = from ?? DateTime.Today.AddDays(-7);
            var toDate = to ?? DateTime.Today;

            var data = _orderService.GetRevenueByDateRange(fromDate, toDate);

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> ExportOrders(DateTime fromDate, DateTime toDate)
        {
            var fileBytes = await _orderService.ExportOrdersToExcelAsync(fromDate, toDate);
            var fileName = $"HoaDon_{fromDate:yyyyMMdd}_{toDate:yyyyMMdd}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}
