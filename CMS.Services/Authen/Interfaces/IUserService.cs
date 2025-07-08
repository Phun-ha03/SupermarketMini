using CMS.BaseModels.Common;
using CMS.Models.Authen.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<UserViewModel>> AuthenticateV2(LoginRequest request);
        Task<ApiResult<string>> SignOut();
        Task<ApiResult<UserViewModel>> Register(RegisterRequest request);
        Task<ApiResult<UserViewModel>> Create(UserCreateRequest request);
        Task<ApiResult<UserViewModel>> Update(UserEditRequest request);
        Task<ApiResult<UserViewModel>> UpdateProfile(UpdateProfileRequest request);
        Task<ApiResult<UserViewModel>> ChangePassword(ChangePasswordRequest request);
        Task<ApiResult<UserViewModel>> ResetPassword(ResetPasswordRequest request);
        Task<ApiResult<string>> ForgotPassword(ForgotPassWordRequest request);
        Task<ApiResult<string>> ChangeAvatar(ChangeAvatarRequest request);
        Task<ApiResult<string>> ChangeCoverPhoto(ChangeCoverPhotoRequest request);
        Task<ApiResult<bool>> Delete(int userId);
        Task<ApiResult<UserViewModel>> GetById(int userId);
        Task<ApiResult<UserViewModel>> GetByIdToDelete(int userId);
        Task<ApiResult<List<UserViewModel>>> GetAll();
        Task<ApiResult<List<UserViewModel>>> GetByIdList(List<int> ids);
        Task<ApiResult<List<UserViewModel>>> GetAllActive();
        Task<ApiResult<PagedResult<UserViewModel>>> GetListPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> AssignRole(UserAssignRoleRequest request);
        Task<ApiResult<bool>> AssignFunction(UserAssignFunctionRequest request);
        Task<ApiResult<UserAssignRoleRequest>> GetUserRoles(int userId);
        Task<ApiResult<UserAssignFunctionRequest>> GetUserFunctions(int userId);
    }
}
