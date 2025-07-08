using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Suppliers
{
    public class SupplierViewModel
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreateAt { get; set; }

        public SupplierViewModel() { }
        public SupplierViewModel(Supplier supplier)
        {
            SupplierID = supplier.SupplierID;
            SupplierName = supplier.SupplierName;
            ContactPerson = supplier.ContactPerson;
            Phone = supplier.Phone;
            Email = supplier.Email;
            Address = supplier.Address;
            CreateAt = DateTime.Now;
        }
    }
}
