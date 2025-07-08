using CMS.BaseModels.Common;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface ICustomerService
    {
        
            Task<ApiResult<CustomerViewModel>> GetById(int id);
            Task<ApiResult<List<CustomerViewModel>>> GetAll();
            Task<ApiResult<CustomerViewModel>> Create(CustomerCreateRequest request);
            Task<ApiResult<CustomerViewModel>> Update(CustomerEditRequest request);
            Task<ApiResult<int>> Delete(int Guid);
            Task<ApiResult<PagedResult<CustomerViewModel>>> GetListPaging(GetCustomerPagingRequest request);
        Task<ApiResult<CustomerViewModel>> GetCustomerByPhone(string phone);
    }
}
