using CMS.BaseModels.Common;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.StockImports;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface IStockImportService
    {

        Task<ApiResult<StockImportViewModel>> GetById(int id);
        Task<ApiResult<List<StockImportViewModel>>> GetAll();
        //Task<bool> Create(StockImportCreateRequest request);
        Task<ApiResult<StockImportViewModel>> Create(StockImportCreateRequest request);
        Task<ApiResult<int>> Delete(int id);
        Task<ApiResult<PagedResult<StockImportViewModel>>> GetListPaging(GetStockImportPagingRequest request);
        //Task<bool> CreateAsync(StockImportCreateRequest request);
    }
}
