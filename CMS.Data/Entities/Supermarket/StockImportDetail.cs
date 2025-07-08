using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class StockImportDetail
    {
        public int ImportDetailID { get; set; }
        public int StockImportID { get; set; }
        public StockImport StockImport { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int UnitID { get; set; }
        public ProductUnit ProductUnit { get; set; }
        public int Quantity { get; set; }
        public decimal CostPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public decimal UsedQuantity { get; set; } = 0;
        
    }
}
