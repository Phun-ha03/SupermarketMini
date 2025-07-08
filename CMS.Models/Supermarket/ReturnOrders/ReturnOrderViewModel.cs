using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ReturnOrders
{
    public class ReturnOrderViewModel
    {
        public int ReturnID { get; set; }
        public int? OrderID { get; set; } 
        public int? ProductID { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Status { get; set; }
        public ReturnOrderViewModel() { }
        public ReturnOrderViewModel(ReturnOrder returnOrder)
        {
            ReturnID = returnOrder.ReturnID;
            OrderID = returnOrder.OrderID;
            ProductID = returnOrder.ProductID;
            Quantity = returnOrder.Quantity;
            Reason = returnOrder.Reason;
            ReturnDate = returnOrder.ReturnDate;
            Status = returnOrder.Status;

        }
    }
}
