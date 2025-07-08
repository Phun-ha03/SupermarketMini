using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.OrderDetails;
using CMS.Models.Supermarket.StockImportDetails;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Orders
{
    public class OrderCreateRequest
    {
        
        [Display(Name = "Tên khách hàng")]
        public int? CustomerID { get; set; }
        [Display(Name = "Tổng tiền")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Giảm giá")]
        public decimal Discount { get; set; }
        [Display(Name = "Phương thức thanh toán")]
        public string PaymentMethod { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }= DateTime.Now;
        [Display(Name = "Điểm sử dụng")]
        public int? UsedPoints { get; set; } 
        public List<OrderDetailCreateRequest> OrderDetails { get; set; } = new List<OrderDetailCreateRequest>();
       
        public OrderCreateRequest() { }
    }
}
