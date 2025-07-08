using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Customers
{
    public class CustomerDeleteRequest
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public CustomerDeleteRequest() { }
        public CustomerDeleteRequest(Customer customer)
        {
            CustomerID = customer.CustomerID;
            Name = customer.Name;
            Email = customer.Email;
        }
        public CustomerDeleteRequest(CustomerViewModel customer)
        {
            CustomerID = customer.CustomerID;
            Name = customer.Name;
            Email = customer.Email;
        }
    }
}
