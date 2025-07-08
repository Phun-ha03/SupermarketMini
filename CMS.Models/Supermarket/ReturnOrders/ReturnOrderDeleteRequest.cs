using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ReturnOrders
{
    public class ReturnOrderDeleteRequest
    {
        public int ReturnID { get; set; }
        //public int? OrderID { get; set; }
        //public int? ProductID { get; set; }
        public ReturnOrderDeleteRequest() { }
        public ReturnOrderDeleteRequest(ReturnOrder returnOrder) 
        { 
            ReturnID = returnOrder.ReturnID;
            //OrderID = returnOrder.OrderID;
            //ProductID = returnOrder.ProductID;

        }
        public ReturnOrderDeleteRequest(ReturnOrderViewModel returnOrder)
        {
            ReturnID = returnOrder.ReturnID;
            //OrderID = returnOrder.OrderID;
            //ProductID = returnOrder.ProductID;
        }
    }
}
