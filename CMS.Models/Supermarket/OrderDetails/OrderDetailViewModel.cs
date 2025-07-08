using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.OrderDetails
{
    public class OrderDetailViewModel
    {

        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitID { get; set; }
        public string UnitName { get; set; }
        public int OrderDetailID { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;


        public OrderDetailViewModel() { }
        public OrderDetailViewModel(OrderDetail orderDetail)
        {
           
            ProductID = orderDetail.ProductID;
            Quantity = orderDetail.Quantity;
            UnitPrice = orderDetail.UnitPrice;
            UnitID = orderDetail.UnitID;
            Name = orderDetail.Product?.Name;
            UnitName = orderDetail.ProductUnit?.UnitName;
        }

    }
}
