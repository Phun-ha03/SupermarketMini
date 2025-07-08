using CMS.BaseModels.Common;
using CMS.Models.Authen.Icons;
using CMS.Models.Authen.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IIconService
    {
        Task<ApiResult<IconViewModel>> GetById(int id);
        Task<ApiResult<List<IconViewModel>>> GetAll();
        Task<ApiResult<List<IconViewModel>>> GetAllActive();
        Task<ApiResult<PagedResult<IconViewModel>>> GetListPaging(GetIconPagingRequest request);
        Task<ApiResult<IconViewModel>> Create(IconCreateRequest request);
        Task<ApiResult<IconViewModel>> Update(IconEditRequest request);
        Task<ApiResult<int>> Delete(int id);
        Task GetListPaging(GetUserPagingRequest request);
    }
}
