using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.OrderDetails
{
    public class OrderDetailCreateRequest
    {
     
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitID { get; set; }

        public OrderDetailCreateRequest() { }
    }
}
