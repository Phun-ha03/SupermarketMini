using CMS.BaseModels.Common;
using CMS.Models.Authen.RoleClaims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IRoleClaimService
    {
        Task<ApiResult<RoleClaimViewModel>> GetById(int id);
        Task<ApiResult<List<RoleClaimViewModel>>> GetAll(int roleId);
        Task<ApiResult<PagedResult<RoleClaimViewModel>>> GetListPaging(GetRoleClaimPagingRequest request);
        Task<ApiResult<RoleClaimViewModel>> Create(RoleClaimCreateRequest request);
        Task<ApiResult<RoleClaimViewModel>> Update(RoleClaimEditRequest request);
        Task<ApiResult<int>> Delete(int id);
    }
}
