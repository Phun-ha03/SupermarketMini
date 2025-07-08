using CMS.BaseModels.Common;
using CMS.Models.Supermarket.StockImportDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface IStockImportDetailService
    {
        Task<ApiResult<StockImportDetailViewModel>> GetById(int id);
        Task<ApiResult<List<StockImportDetailViewModel>>> GetAll();
        Task<ApiResult<StockImportDetailViewModel>> Create(StockImportDetailCreateRequest request);
        Task<ApiResult<int>> Delete(int id);
        Task<List<StockImportDetailViewModel>> GetDetailsByImportId(int importId);
        Task<ApiResult<PagedResult<StockImportDetailViewModel>>> GetListPaging(GetStockImportDetailPagingRequest request);
    }

}
