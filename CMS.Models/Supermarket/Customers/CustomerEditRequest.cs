using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Customers
{
    public class CustomerEditRequest
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
        [Display(Name = "Tích điểm")]
        public int LoyalPoints { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }
         public CustomerEditRequest() { }
        public CustomerEditRequest(Customer customer)
        {
            CustomerID = customer.CustomerID;
            Name = customer.Name;
            Phone = customer.Phone;
            Email = customer.Email;
            Address = customer.Address;
            LoyalPoints = customer.LoyalPoints;
           // CreateAt = DateTime.Now;
        }
        public CustomerEditRequest(CustomerViewModel customer)
        {
            CustomerID = customer.CustomerID;
            Name = customer.Name;
            Phone = customer.Phone;
            Email = customer.Email;
            Address = customer.Address;
            LoyalPoints = customer.LoyalPoints;
           // CreateAt = DateTime.Now;

        }
    }

}
