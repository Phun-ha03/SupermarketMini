using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class StockImport
    {
        public int StockImportID { get; set; }
        public int SupplierID { get; set; }
        public Supplier Supplier { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime ImportDate { get; set; } = DateTime.Now;
        public ICollection<StockImportDetail> StockImportDetails { get; set; } = new List<StockImportDetail>();
        public decimal? DiscountAmount { get; set; }
       
    }
}
