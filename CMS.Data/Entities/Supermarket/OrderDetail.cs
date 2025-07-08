using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int UnitID { get; set; }
        public ProductUnit ProductUnit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
      
    }
}
