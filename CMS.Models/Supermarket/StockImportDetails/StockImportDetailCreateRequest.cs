using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImportDetails
{
    public class StockImportDetailCreateRequest
    {
        //public int ImportDetailID { get; set; }
        //public int ImportID { get; set; }
        public int ProductID { get; set; } 
        //public string UnitName { get;  set; }
        public int? UnitID { get; set;}
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }

        public DateTime? ExpirationDate { get; set; } 
        public StockImportDetailCreateRequest() { }

    }
}
