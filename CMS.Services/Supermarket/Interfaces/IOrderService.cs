using CMS.BaseModels.Common;
using CMS.Models.Supermarket.Orders;
using CMS.Models.Supermarket.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface IOrderService
    {
        Task<ApiResult<OrderViewModel>> GetById(int id);
        Task<ApiResult<List<OrderViewModel>>> GetAll();
        Task<ApiResult<OrderViewModel>> Create(OrderCreateRequest request, int currentUserId);
        Task<ApiResult<int>> Delete(int Guid);
        Task<ApiResult<PagedResult<OrderViewModel>>> GetListPaging(GetOrderPagingRequest request);
        List<RevenueByDateDto> GetRevenueByDateRange(DateTime fromDate, DateTime toDate);
        Task<byte[]> ExportOrdersToExcelAsync(DateTime fromDate, DateTime toDate);
    }
}
