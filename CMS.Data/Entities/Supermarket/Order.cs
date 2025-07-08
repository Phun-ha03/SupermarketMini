using CMS.Data.Entities.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class Order
    {
        public int OrderID { get; set; }
        public int? CustomerID { get; set; }
        public Customer Customer { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount {  get; set; }
        public string PaymenMethod { get; set; }
        public string Status { get; set; }
        public int UsedPoints { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public int? CreatedBy { get; set; }            // ID của người tạo đơn
        public User CreatedUser { get; set; }          // Tham chiếu đến user

    }
}
