using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreateAt { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
