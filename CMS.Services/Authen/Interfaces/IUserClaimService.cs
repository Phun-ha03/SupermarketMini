using CMS.BaseModels.Common;
using CMS.Models.Authen.UserClaims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IUserClaimService
    {
        Task<ApiResult<UserClaimViewModel>> GetById(int id);
        Task<ApiResult<List<UserClaimViewModel>>> GetAll(int userId);
        Task<ApiResult<PagedResult<UserClaimViewModel>>> GetListPaging(GetUserClaimPagingRequest request);
        Task<ApiResult<UserClaimViewModel>> Create(UserClaimCreateRequest request);
        Task<ApiResult<UserClaimViewModel>> Update(UserClaimEditRequest request);
        Task<ApiResult<int>> Delete(int id);
    }
}
