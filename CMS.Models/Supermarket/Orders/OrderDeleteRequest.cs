using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Orders
{
    public class OrderDeleteRequest
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
       
        public OrderDeleteRequest() { }
        public OrderDeleteRequest(Order order)
        {
            OrderID = order.OrderID;
            CustomerID = order.CustomerID;
           
        }
        public OrderDeleteRequest(OrderViewModel order )
        {
            OrderID = order.OrderID;
            CustomerID = order.CustomerID;
            
        }
    }
}
