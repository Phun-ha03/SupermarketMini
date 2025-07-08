using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.StockImportDetails;
using CMS.Models.Supermarket.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImports
{
    public class StockImportViewModel
    {
        public int StockImportID { get; set; }
        public int SupplierID { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? ImportDate { get; set; } 
        public string SupplierName { get; set; }

        public List<StockImportDetailViewModel> StockImportDetails { get; set; } = new();
       
        public StockImportViewModel() { }
        public StockImportViewModel(StockImport stockImport)
        {
            StockImportID = stockImport.StockImportID;
            SupplierID = stockImport.SupplierID;
            TotalCost = stockImport.TotalCost;
            ImportDate = stockImport.ImportDate;
            StockImportDetails= new();
            SupplierName = stockImport.Supplier != null ? stockImport.Supplier.SupplierName : "";
        }
    }
}
