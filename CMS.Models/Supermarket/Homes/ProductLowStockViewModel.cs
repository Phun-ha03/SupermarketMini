using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Homes
{
    public class ProductLowStockViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int StockQuantity { get; set; }
    }
}
