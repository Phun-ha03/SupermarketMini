using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Suppliers;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.WebApp.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger,
            IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IProductService productService,
            IUserService userService,
            IHomeService homeService
            ) : base(functionService, httpContextAccessor, userService)
        {
            _logger = logger;
            _configuration = configuration;

            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _categoryService = categoryService;
            _supplierService = supplierService;
            _productService = productService;
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            

            return View();
        }
        //public ActionResult DailyRevenue(DateTime? fromDate, DateTime? toDate, string filterType = "day")
        //{
        //    DateTime from = fromDate ?? DateTime.Today.AddDays(-7);
        //    DateTime to = toDate ?? DateTime.Today;

        //    ViewBag.FromDate = from;
        //    ViewBag.ToDate = to;
        //    ViewBag.FilterType = filterType;

        //    var data = _homeService.GetRevenueReport(from, to, filterType);
        //    return View(data);
        //}

        //public IActionResult BestSellingProducts(DateTime? fromDate, DateTime? toDate)
        //{
        //    ViewBag.FromDate = fromDate ?? DateTime.Today.AddDays(-30);
        //    ViewBag.ToDate = toDate ?? DateTime.Today;

        //    var result = _homeService.GetBestSellingProducts(ViewBag.FromDate, ViewBag.ToDate);
        //    return View(result);
        //}
        public IActionResult Dashboard(DateTime? fromDate, DateTime? toDate, string filterType = "day")
        {
            var from = fromDate ?? DateTime.Today.AddDays(-7);
            var to = toDate ?? DateTime.Today;

            var model = _homeService.GetDashboardReport(from, to, filterType);
            return View(model);
        }

    }
}