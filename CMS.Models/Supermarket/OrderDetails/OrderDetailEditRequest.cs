using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.OrderDetails
{
    public class OrderDetailEditRequest
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int OrderDetailID { get; set; }

        public OrderDetailEditRequest() { }
        public OrderDetailEditRequest(OrderDetail orderDetail)
        {
            OrderID = orderDetail.OrderID;
            ProductID = orderDetail.ProductID;
            Quantity = orderDetail.Quantity;
            UnitPrice = orderDetail.UnitPrice;

        }
        public OrderDetailEditRequest(OrderDetailViewModel orderDetail)
        {
            OrderID = orderDetail.OrderID;
            ProductID = orderDetail.ProductID;
            Quantity = orderDetail.Quantity;
            UnitPrice = orderDetail.UnitPrice;
            
        }
    }
}
