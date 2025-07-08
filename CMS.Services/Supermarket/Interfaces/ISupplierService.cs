using CMS.BaseModels.Common;
using CMS.Models.Supermarket.Suppliers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface ISupplierService
    {
        Task<ApiResult<SupplierViewModel>> GetById(int id);
        Task<ApiResult<List<SupplierViewModel>>> GetAll();
        Task<ApiResult<SupplierViewModel>> Create(SupplierCreateRequest request);
        Task<ApiResult<SupplierViewModel>> Update(SupplierEditRequest request);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<PagedResult<SupplierViewModel>>> GetListPaging(GetSupplierPagingRequest request);
        Task<List<SelectListItem>> GetSelectListAsync();
        Task<byte[]> ExportSuppliersToExcelAsync();
    }
}
