using CMS.BaseModels.Common;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Authen.Functions;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.ProductUnits;
using CMS.Models.Supermarket.Suppliers;
using CMS.Services.Authen.Interfaces;
using CMS.Services.Supermarket;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.WebApp.Controllers
{
    public class ProductUnitController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IFunctionService _functionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        private readonly IProductUnitService _productUnitService;
        private readonly IProductService _productService;

        public ProductUnitController(IConfiguration configuration,
            IFunctionService functionService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IProductUnitService productUnitService,
            IProductService productService
            ) : base(functionService, httpContextAccessor, userService)
        {
            _configuration = configuration;
            _functionService = functionService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _productUnitService = productUnitService;
            _productService = productService;
        }

        public async Task AddProductSelectItemsToViewBag(int selectedId)
        {
            try
            {
                var products = (await _productService.GetAll()).ResultObj;
                products = products ?? new List<ProductViewModel>();

                var defaultProduct = new ProductViewModel()
                {
                    ProductID = 0,
                    Name = "Chọn sản phẩm"
                };

                products.Insert(0, defaultProduct);

                ViewBag.Products = new SelectList(products, "ProductID", "Name", selectedId);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
        [HttpGet("ProductUnit/index")]
        public async Task<IActionResult> Index(string keyword, int? productID,int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
        {
            try
            {
                keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
                ViewBag.Keyword = keyword;
                ViewBag.ProductID = productID ?? 0;

                await AddProductSelectItemsToViewBag(ViewBag.ProductID);

                var request = new GetProductUnitPagingRequest()
                {
                    Keyword = keyword,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };

                var result = await _productUnitService.GetListPaging(request);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.LoadingError : result.Message;
                    return View(new PagedResult<ProductUnitViewModel>() { Items = new List<ProductUnitViewModel>() }); // Trả về đúng kiểu model
                }

                return View(result.ResultObj);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpGet("ProductUnit/create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                await AddProductSelectItemsToViewBag(0);
                return View();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductUnitCreateRequest request)
        {
            try
            {
                await AddProductSelectItemsToViewBag((byte)(request.ProductID));
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _productUnitService.Create(request);

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

        [HttpGet("ProductUnit/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var result = await _productUnitService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    await AddProductSelectItemsToViewBag(0);
                    return View();
                }

                await AddProductSelectItemsToViewBag((result.ResultObj.ProductID ?? 0));
                return View(new ProductUnitEditRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUnitEditRequest request)
        {
            try
            {
                await AddProductSelectItemsToViewBag(request.ProductID ?? 0);
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var result = await _productUnitService.Update(request);

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

        [HttpGet("ProductUnit/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _productUnitService.GetById(id);

                if (!result.IsSuccessed)
                {
                    ViewBag.Error = result.Message;
                    return View();
                }

                return View(new ProductUnitDeleteRequest(result.ResultObj));
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductUnitDeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var result = await _productUnitService.Delete(request.UnitID);

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



//[HttpGet("ProductUnit/index")]
//public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = ConstantHelper.PageSize)
//{
//    try
//    {
//        keyword = string.IsNullOrEmpty(keyword) ? string.Empty : keyword;
//        ViewBag.Keyword = keyword;

//        var request = new GetProductUnitPagingRequest()
//        {
//            Keyword = keyword,
//            PageIndex = pageIndex,
//            PageSize = pageSize
//        };

//        var result = await _ProductUnitService.GetListPaging(request);

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

//[HttpGet("ProductUnit/create")]
//public async Task<IActionResult> Create()
//{
//    try
//    {
//        return View();
//    }
//    catch (Exception ex)
//    {
//        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
//        throw;
//    }
//}

//[HttpPost]
//public async Task<IActionResult> Create(ProductUnitCreateRequest request)
//{
//    try
//    {
//        if (!ModelState.IsValid)
//        {
//            return View(request);
//        }

//        var result = await _ProductUnitService.Create(request);

//        if (result.IsSuccessed)
//        {
//            ViewBag.Message = ConstantHelper.InsertSuccess;
//            return View(request);
//        }

//        ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

//        return View(request);
//    }
//    catch (Exception ex)
//    {
//        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
//        throw;
//    }
//}

//[HttpGet("ProductUnit/edit/{id}")]
//public async Task<IActionResult> Edit(int id)
//{
//    try
//    {
//        var result = await _ProductUnitService.GetById(id);

//        if (!result.IsSuccessed)
//        {
//            ViewBag.Error = result.Message;
//            return View();
//        }

//        return View(new ProductUnitEditRequest(result.ResultObj));
//    }
//    catch (Exception ex)
//    {
//        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
//        throw;
//    }
//}

//[HttpPost]
//public async Task<IActionResult> Edit(ProductUnitEditRequest request)
//{
//    try
//    {
//        if (!ModelState.IsValid)
//        {
//            return View(request);
//        }

//        var result = await _ProductUnitService.Update(request);

//        if (result.IsSuccessed)
//        {
//            ViewBag.Message = ConstantHelper.UpdateSuccess;
//            return View(request);
//        }

//        ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;

//        return View(request);
//    }
//    catch (Exception ex)
//    {
//        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
//        throw;
//    }
//}

//[HttpGet("ProductUnit/delete/{id}")]
//public async Task<IActionResult> Delete(int id)
//{
//    try
//    {
//        var result = await _ProductUnitService.GetById(id);

//        if (!result.IsSuccessed)
//        {
//            ViewBag.Error = result.Message;
//            return View();
//        }

//        return View(new ProductUnitDeleteRequest(result.ResultObj));
//    }
//    catch (Exception ex)
//    {
//        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
//        throw;
//    }
//}

//[HttpPost]
//public async Task<IActionResult> Delete(ProductUnitDeleteRequest request)
//{
//    try
//    {
//        if (!ModelState.IsValid)
//        {
//            return View();
//        }

//        var result = await _ProductUnitService.Delete(request.UnitID);

//        if (result.IsSuccessed)
//        {
//            ViewBag.Message = ConstantHelper.DeleteSuccess;
//        }
//        else
//        {
//            ViewBag.Error = string.IsNullOrEmpty(result.Message) ? ConstantHelper.UpdateError : result.Message;
//        }

//        return View(request);
//    }
//    catch (Exception ex)
//    {
//        LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
//        throw;
//    }
//}
