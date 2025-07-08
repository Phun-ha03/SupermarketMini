using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImportDetails
{
    public class StockImportDetailEditRequest
    {
        public int ImportDetailID { get; set; }
        public int StockImportID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public StockImportDetailEditRequest() { }
        public StockImportDetailEditRequest(StockImportDetail stockImportDetail)
        {
            ImportDetailID = stockImportDetail.ImportDetailID;
            StockImportID = stockImportDetail.StockImportID;
            ProductID = stockImportDetail.ProductID;
            Quantity = stockImportDetail.Quantity;
            CostPrice = stockImportDetail.CostPrice;
            ExpirationDate = stockImportDetail.ExpirationDate;
        }
        public StockImportDetailEditRequest(StockImportDetailViewModel stockImportDetail)
        {
            ImportDetailID = stockImportDetail.ImportDetailID;
            StockImportID= stockImportDetail.StockImportID;
            ProductID = stockImportDetail.ProductID;
            Quantity = stockImportDetail.Quantity;
            CostPrice = stockImportDetail.CostPrice;
            ExpirationDate = stockImportDetail.ExpirationDate;
        }
    }
}
