using CMS.BaseModels.Common;
using CMS.Models.Supermarket.ProductUnits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface IProductUnitService
    {
        Task<ApiResult<ProductUnitViewModel>> GetById(int id);
        Task<ApiResult<List<ProductUnitViewModel>>> GetAll();
        Task<ApiResult<ProductUnitViewModel>> Create(ProductUnitCreateRequest request);
        Task<ApiResult<ProductUnitViewModel>> Update(ProductUnitEditRequest request);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<PagedResult<ProductUnitViewModel>>> GetListPaging(GetProductUnitPagingRequest request);
       // Task<List<ProductUnitViewModel>> GetDetailsByUnitsId(int importId);
    }
}
