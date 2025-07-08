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
    public class CartController : BaseController
    {
        private readonly ILogger<CartController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;

        public CartController(ILogger<CartController> logger,
            IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            ICategoryService categoryService,
            ISupplierService supplierService,
            IProductService productService,
            IUserService userService
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
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {


            return View();
        }
    }
}