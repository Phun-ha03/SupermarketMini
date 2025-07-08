using CMS.Services.Authen.Interfaces;
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

        public HomeController(ILogger<HomeController> logger,
            IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService
            ) : base(functionService, httpContextAccessor, userService)
        {
            _logger = logger;
            _configuration = configuration;

            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();

        }
    }
}