using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Suppliers
{
    public class SupplierDeleteRequest
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string ContactPerson { get; set; }
       

        public SupplierDeleteRequest() { }
         public SupplierDeleteRequest(Supplier supplier)
        {
            SupplierID = supplier.SupplierID;
            SupplierName = supplier.SupplierName;
            ContactPerson = supplier.ContactPerson;
           
        }
        public SupplierDeleteRequest(SupplierViewModel supplier)
        {
            SupplierID = supplier.SupplierID;
            SupplierName = supplier.SupplierName;
            ContactPerson = supplier.ContactPerson;
           
        }
    }
}
