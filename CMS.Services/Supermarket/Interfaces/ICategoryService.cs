using CMS.BaseModels.Common;
using CMS.Models.Supermarket.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResult<CategoryViewModel>> GetById(int id);
        Task<ApiResult<List<CategoryViewModel>>> GetAll();
        Task<ApiResult<CategoryViewModel>> Create(CategoryCreateRequest request);
        Task<ApiResult<CategoryViewModel>> Update(CategoryEditRequest request);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<PagedResult<CategoryViewModel>>> GetListPaging(GetCategoryPagingRequest request);
        Task<byte[]> ExportCategoriesToExcelAsync();
    }
}
