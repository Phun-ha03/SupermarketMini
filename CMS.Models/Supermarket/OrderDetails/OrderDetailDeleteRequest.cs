using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.OrderDetails
{
    public class OrderDetailDeleteRequest
    {
        public int OrderDetailID {  get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public OrderDetailDeleteRequest() { }
        public OrderDetailDeleteRequest(OrderDetail orderDetail)
        {
            OrderDetailID =orderDetail.OrderDetailID ;
            OrderID = orderDetail.OrderID;
            ProductID = orderDetail.ProductID;
            Quantity = orderDetail.Quantity;
            UnitPrice = orderDetail.UnitPrice;
            

        }
        public OrderDetailDeleteRequest(OrderDetailViewModel orderDetail)
        {
            OrderID = orderDetail.OrderID;
            ProductID = orderDetail.ProductID;
            Quantity = orderDetail.Quantity;
            UnitPrice = orderDetail.UnitPrice;
        }
    }
}
