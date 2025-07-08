using CMS.Models.Authen.Functions;
using CMS.Models.Supermarket.Suppliers;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApp.Controllers
{
    public class SupplierController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        private readonly ISupplierService _supplierService;

        public SupplierController(IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            ISupplierService SupplierService) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _supplierService = SupplierService;
        }

        [HttpGet("Supplier/index")]
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                ViewBag.Keyword = keyword;

                var request = new GetSupplierPagingRequest()
                {
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                var result = await _supplierService.GetListPaging(request);

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

        [HttpGet("Supplier/create")]
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
        public async Task<IActionResult> Create(SupplierCreateRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _supplierService.Create(request);

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

        [HttpGet("Supplier/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _supplierService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new SupplierEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SupplierEditRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _supplierService.Update(request);

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

        [HttpGet("Supplier/delete/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var result = await _supplierService.GetById(id);

        //        if (!result.IsSuccessed)
        //        {
        //            ViewBag.Error = result.Message;
        //            return View();
        //        }

        //        return View(new SupplierDeleteRequest()
        //        {
        //            SupplierID= result.ResultObj.SupplierID,

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
        //        throw;
        //    }
        //}
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _supplierService.GetById(id);

                if (!result.IsSuccessed || result.ResultObj == null)
                {
                    ViewBag.Error = "Nhà cung cấp không tồn tại!";
                    return RedirectToAction("Index"); // Điều hướng thay vì trả về View rỗng
                }

                return View(new SupplierDeleteRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SupplierDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _supplierService.Delete(request.SupplierID);

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
        public async Task<IActionResult> ExportSuppliersToExcel()
        {
            var content = await _supplierService.ExportSuppliersToExcelAsync();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachNhaCungCap.xlsx");
        }

    }
}

