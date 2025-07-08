using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email     { get; set; }
        public string Address { get; set; }
        public int LoyalPoints { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
