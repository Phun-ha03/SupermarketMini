using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ProductUnits
{
    public class ProductUnitViewModel
    {
        public int UnitID { get; set; }
        public int? ProductID { get; set; }
        public string UnitName { get; set; }
        public decimal ConversionRate { get; set; }
        public Decimal? UnitPrice { get; set; }
        public bool IsBaseUnit { get; set; }
        public string Name { get; set; }
        public  Product Product { get; set; }
        public string ProductCode { get; set; }
        public List<ProductViewModel> Products { get; set; } = new List<ProductViewModel>();
        public ProductUnitViewModel() { }
        public ProductUnitViewModel(ProductUnit productUnit)
        {
            UnitID = productUnit.UnitID;
            ProductID = productUnit.ProductID;
            UnitName = productUnit.UnitName;
            ConversionRate = productUnit.ConversionRate;
            UnitPrice = productUnit.UnitPrice;
            Name = productUnit.Product != null ? productUnit.Product.Name : "";
            ProductCode = productUnit.Product != null ? productUnit.Product.ProductCode : "";
        }

    }
}
