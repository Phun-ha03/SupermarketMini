using CMS.BaseModels.Common;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IRoleService
    {
        Task<ApiResult<RoleViewModel>> GetById(int id);
        Task<ApiResult<List<RoleViewModel>>> GetListByUser(int userId);
        Task<ApiResult<RoleViewModel>> GetWithClaimsById(int id);
        Task<ApiResult<RoleViewModel>> GetWithFunctionsById(int id);
        Task<ApiResult<List<RoleViewModel>>> GetAll();
        Task<ApiResult<List<RoleViewModel>>> GetHỉerachy(GetRoleHierarchyRequest request);
        Task<ApiResult<List<RoleViewModel>>> GetUserMenus(GetUserMenuRequest request);
        List<RoleViewModel> BuildUserMenus(List<Role> roles);
        Task<ApiResult<PagedResult<RoleViewModel>>> GetListPaging(GetRolePagingRequest request);
        Task<ApiResult<RoleViewModel>> Create(RoleCreateRequest request);
        Task<ApiResult<RoleViewModel>> Update(RoleEditRequest request);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<string>> AssignFunction(RoleAssignFunctionRequest request);
    }
}
