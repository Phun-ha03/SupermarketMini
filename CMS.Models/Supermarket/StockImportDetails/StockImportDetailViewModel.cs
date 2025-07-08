using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Products;
using CMS.Models.Supermarket.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImportDetails
{
    public class StockImportDetailViewModel
    {
        public int ImportDetailID { get; set; }
        public int StockImportID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? UnitID { get; set; }
        public string Name { get; set; }
        public decimal UsedQuantity { get; set; }
        public StockImportDetailViewModel() { }
        public StockImportDetailViewModel(StockImportDetail stockImportDetail)
        {
            ImportDetailID = stockImportDetail.ImportDetailID;
            StockImportID = stockImportDetail.StockImportID;
            ProductID = stockImportDetail.ProductID;
            Quantity = stockImportDetail.Quantity;
            CostPrice = stockImportDetail.CostPrice;
            ExpirationDate = stockImportDetail.ExpirationDate;
            UnitID=stockImportDetail.UnitID;
            UsedQuantity= stockImportDetail.UsedQuantity;
        }
    }
}
