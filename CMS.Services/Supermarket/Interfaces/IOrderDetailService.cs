using CMS.BaseModels.Common;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.OrderDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface IOrderDetailService
    {
        Task<ApiResult<OrderDetailViewModel>> GetById(int id);
        Task<ApiResult<List<OrderDetailViewModel>>> GetAll();
        Task<ApiResult<OrderDetailViewModel>> Create(OrderDetailCreateRequest request);
        Task<ApiResult<OrderDetailViewModel>> Update(OrderDetailEditRequest request);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<PagedResult<OrderDetailViewModel>>> GetListPaging(GetOrderDetailPagingRequest request);
        Task<List<OrderDetailViewModel>> GetDetailsByOrderId(int orderId);
    }
}
