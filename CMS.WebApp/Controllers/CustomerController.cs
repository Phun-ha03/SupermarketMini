using CMS.Models.Authen.Functions;
using CMS.Models.Supermarket.Customers;
using CMS.Models.Supermarket.Customers;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApp.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        private readonly ICustomerService _customerService;

        public CustomerController(IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            ICustomerService customerService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _customerService = customerService;
        }

        [HttpGet("Customer/index")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                ViewBag.Keyword = keyword;

                var request = new GetCustomerPagingRequest()
                {
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                var result = await _customerService.GetListPaging(request);

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

        [HttpGet("Customer/create")]
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
        public async Task<IActionResult> Create(CustomerCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _customerService.Create(request);

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

        [HttpGet("Customer/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _customerService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new CustomerEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CustomerEditRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _customerService.Update(request);

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

        [HttpGet("Customer/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _customerService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new CustomerDeleteRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CustomerDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _customerService.Delete(request.CustomerID);

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
    }
}
