using CMS.BaseModels.Common;
using CMS.Models.Authen.UserLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IUserLogService
    {
        Task<ApiResult<UserLogViewModel>> Create(UserLogCreateRequest request);
        Task<ApiResult<int>> Delete(int id);
        Task<ApiResult<List<UserLogViewModel>>> GetAll();
        Task<ApiResult<List<UserLogViewModel>>> GetAllActive();
        Task<ApiResult<UserLogViewModel>> GetById(int id);
        Task<ApiResult<PagedResult<UserLogViewModel>>> GetListPaging(GetUserLogPagingRequest request);
        Task<ApiResult<UserLogViewModel>> Update(UserLogEditRequest request);
    }
}
