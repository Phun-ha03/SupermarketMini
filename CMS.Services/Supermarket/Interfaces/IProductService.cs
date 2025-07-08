
using CMS.BaseModels.Common;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.ProductUnits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface IProductService
    {
        Task<ApiResult<ProductViewModel>> GetById(int id);
        Task<ApiResult<List<ProductViewModel>>> GetAll();
       // Task<ApiResult<ProductViewModel>> CreateV2(ProductCreateRequest request);
        Task<ApiResult<bool>> Create(ProductCreateRequest model);
        // Task<ApiResult<ProductViewModel>> Update(ProductEditRequest request);
        Task<ApiResult<bool>> Update(ProductEditRequest request);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<PagedResult<ProductViewModel>>> GetListPaging(GetProductPagingRequest request);
        Task<byte[]> ExportCsv();
        //Task<ApiResult<string>> SaveImageAsync(IFormFile file);
        Task<ApiResult<List<ProductViewModel>>> GetProductsByKeyword(string keyword);
        Task<List<ProductUnitViewModel>> GetUnitsByProductID(int productID);
        Task<byte[]> ExportProductsToExcelAsync();

    }
}
