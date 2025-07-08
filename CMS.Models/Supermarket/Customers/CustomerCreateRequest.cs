using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Customers
{
    public class CustomerCreateRequest
    {
        public int CustomerID { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string Name { get; set; }
        [Display(Name = "SDT")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Tích Điểm")]
        public int LoyalPoints { get; set; }
        [Display(Name = "Ngày thêm")]
        public DateTime CreateAt { get; set; }
        public CustomerCreateRequest() { }
    }
}
