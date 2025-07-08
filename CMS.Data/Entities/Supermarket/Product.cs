using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class Product
    {
        public int ProductID { get; set; }
       
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal StockQuantity { get; set; }
        public string Barcode { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? CategoryID { get; set; }
        public Category Category { get; set; }
        public int? SupplierID { get; set; }
        public Supplier Supplier { get; set; }
        public string? ProductCode { get; set; }
        public string? Image {  get; set; }
        public  ICollection<ProductUnit> ProductUnits { get; set; } = new List<ProductUnit>();


    }
}
