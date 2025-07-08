using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Homes
{
    public class ProductExpireSoonViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal StockQuantity { get; set; }
    }
}
