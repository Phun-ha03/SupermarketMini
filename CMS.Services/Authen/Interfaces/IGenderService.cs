using CMS.BaseModels.Common;
using CMS.Models.Authen.Genders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Authen.Interfaces
{
    public interface IGenderService
    {
        Task<ApiResult<GenderViewModel>> GetById(int id);
        Task<ApiResult<List<GenderViewModel>>> GetAll();
        Task<ApiResult<PagedResult<GenderViewModel>>> GetListPaging(GetGenderPagingRequest request);
        Task<ApiResult<GenderViewModel>> Create(GenderCreateRequest request);
        Task<ApiResult<GenderViewModel>> Update(GenderEditRequest request);
        Task<ApiResult<int>> Delete(int id);
    }
}
