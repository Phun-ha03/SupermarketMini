using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.OrderDetails;
using CMS.Models.Supermarket.StockImportDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Orders
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public string PaymenMethod { get; set; }
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; } = new();
        public OrderViewModel() { }

        public OrderViewModel(Order order)
        {
            OrderID = order.OrderID;
            CustomerID = order.CustomerID;
            TotalAmount = order.TotalAmount;
            Discount = order.Discount;
            PaymenMethod = order.PaymenMethod;
            //Status = order.Status;
            CreateAt = order.CreateAt;
            if (order.OrderDetails != null)
            {
                OrderDetails = order.OrderDetails
                    .Select(od => new OrderDetailViewModel(od))
                    .ToList();
            }
            Name = order.Customer != null ? order.Customer.Name : "";
        }

    }

}
