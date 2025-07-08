using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImportDetails
{
    public class StockImportDetailDeleteRequest
    {
        public int ImportDetailID { get; set; }
        public int StockImportID { get; set; }
        public StockImportDetailDeleteRequest() { }
        public StockImportDetailDeleteRequest(StockImportDetail stockImportDetail)
        {
            ImportDetailID = stockImportDetail.ImportDetailID;
            StockImportID = stockImportDetail.StockImportID;
        }
        public StockImportDetailDeleteRequest(StockImportDetailViewModel stockImportDetail)
        {
            ImportDetailID = stockImportDetail.ImportDetailID;
            StockImportID = stockImportDetail.StockImportID;
        }
    }
}
