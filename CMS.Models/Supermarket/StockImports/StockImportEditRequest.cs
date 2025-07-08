using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImports
{
    public class StockImportEditRequest
    {
        public int StockImportID { get; set; }
        public int SupplierID { get; set; }
        public decimal? TotalCost { get; set; }
        public DateTime? ImportDate { get; set; }
        public ICollection<StockImportDetail> StockImportDetails { get; set; } = new List<StockImportDetail>();
        public StockImportEditRequest() { }
        public StockImportEditRequest(StockImport stockImport)
        {
            StockImportID=stockImport.StockImportID;
            SupplierID=stockImport.SupplierID;
            TotalCost=stockImport.TotalCost;
            ImportDate=stockImport.ImportDate;

        }
        public StockImportEditRequest(StockImportViewModel stockImport)
        {
            StockImportID=stockImport.StockImportID;
            SupplierID=stockImport.SupplierID;
            TotalCost=stockImport.TotalCost;
            ImportDate=stockImport.ImportDate;
        }
    }
}
