using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Orders
{
    public class OrderEditRequest
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public string PaymenMethod { get; set; }
      //  public string Status { get; set; }
        public DateTime CreateAt { get; set; }
        public OrderEditRequest() { }
        public OrderEditRequest(Order order)
        {
            OrderID = order.OrderID;
            CustomerID = order.CustomerID;
            TotalAmount = order.TotalAmount;
            Discount = order.Discount;
            PaymenMethod = order.PaymenMethod;
           // Status = order.Status;
            CreateAt = order.CreateAt;

        }
        public OrderEditRequest(OrderViewModel order )
        {
            OrderID = order.OrderID;
            CustomerID = order.CustomerID;
            TotalAmount = order.TotalAmount;
            Discount = order.Discount;
            PaymenMethod = order.PaymenMethod;
           // Status = order.Status;
            CreateAt= order.CreateAt;
        }
    }
}
