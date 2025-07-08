using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class ReturnOrder
    {
        public int ReturnID { get; set; }
        public int? OrderID { get; set; }
        public Order Order { get; set; }
        public int? ProductID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime ReturnDate { get; set; } 
        public string Status { get; set; } 
    }
}
