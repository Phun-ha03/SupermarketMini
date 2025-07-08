using CMS.BaseModels.Common;
using CMS.Data.Entities.Authen;
using CMS.Models.Authen.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IFunctionService
    {
        Task<ApiResult<FunctionViewModel>> GetById(int id);
        Task<ApiResult<List<FunctionViewModel>>> GetAll();
        Task<ApiResult<List<FunctionViewModel>>> GetHỉerachy(GetFunctionHierarchyRequest request);
        Task<ApiResult<List<FunctionViewModel>>> GetUserMenus(GetUserMenuRequest request);
        Task<ApiResult<List<FunctionViewModel>>> GetAllActiveUserMenus(GetUserMenuRequest request);
        List<FunctionViewModel> BuildUserMenus(List<Function> functions);
        Task<ApiResult<FunctionViewModel>> Create(FunctionCreateRequest request);
        Task<ApiResult<FunctionViewModel>> Update(FunctionEditRequest request);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<bool>> CheckPermission(int userId, string controller, string action);
    }
}

