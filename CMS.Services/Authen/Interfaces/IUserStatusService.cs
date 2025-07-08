using CMS.BaseModels.Common;
using CMS.Models.Authen.UserStatuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IUserStatusService
    {
        Task<ApiResult<UserStatusViewModel>> GetById(byte id);
        Task<ApiResult<UserStatusViewModel>> GetByIdMax();
        Task<ApiResult<List<UserStatusViewModel>>> GetAll();
        Task<ApiResult<PagedResult<UserStatusViewModel>>> GetListPaging(GetUserStatusPagingRequest request);
        Task<ApiResult<UserStatusViewModel>> Create(UserStatusCreateRequest request);
        Task<ApiResult<UserStatusViewModel>> Update(UserStatusEditRequest request);
        Task<ApiResult<int>> Delete(byte id);
    }
}
