using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Customers
{
    public class CustomerViewModel
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int LoyalPoints { get; set; }
        public DateTime CreateAt { get; set; }
        public CustomerViewModel() { }
        public CustomerViewModel(Customer customer)
        {
            CustomerID = customer.CustomerID;
            Name = customer.Name;
            Phone = customer.Phone;
            Email = customer.Email;
            Address = customer.Address;
            LoyalPoints = customer.LoyalPoints;
            CreateAt = customer.CreateAt;
        }
    }
}
