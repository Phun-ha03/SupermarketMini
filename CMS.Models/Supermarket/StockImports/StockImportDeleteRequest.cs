using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImports
{
    public class StockImportDeleteRequest
    {
        public int StockImportID { get; set; }
        public int SupplierID { get; set; }
       
        public StockImportDeleteRequest() { }
        public StockImportDeleteRequest(StockImport stockImport)
        {
            StockImportID = stockImport.StockImportID;
            SupplierID = stockImport.SupplierID;
        }
        public StockImportDeleteRequest(StockImportViewModel stockImport)
        {
            StockImportID = stockImport.StockImportID;
            SupplierID = stockImport.SupplierID;
        
        }
    }
}
