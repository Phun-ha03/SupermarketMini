using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Products
{
    public class ProductDeleteRequest
    {

        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
       
        public ProductDeleteRequest() { }

        public ProductDeleteRequest(Product product)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            Description = product.Description;
           
        }

        public ProductDeleteRequest(ProductViewModel product)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            Description = product.Description;
           
        }
    }
}
