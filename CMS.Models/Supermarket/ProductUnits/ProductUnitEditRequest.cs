using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ProductUnits
{
    public class ProductUnitEditRequest
    {
        public int UnitID { get; set; }
        public int? ProductID { get; set; }
        public string UnitName { get; set; }
        public decimal ConversionRate { get; set; }
        public Decimal? UnitPrice { get; set; }
        public bool IsBaseUnit { get; set; }
        public ProductUnitEditRequest() { }
        public ProductUnitEditRequest(ProductUnit productUnit)
        {
            UnitID=productUnit.UnitID;
            ProductID = productUnit.ProductID;
            UnitName = productUnit.UnitName;
            ConversionRate = productUnit.ConversionRate;
            UnitPrice = productUnit.UnitPrice;
            IsBaseUnit = productUnit.IsBaseUnit;
        }
        public ProductUnitEditRequest(ProductUnitViewModel productUnit)
        {
            UnitID = productUnit.UnitID;
            ProductID = productUnit.ProductID;
            UnitName = productUnit.UnitName;
            ConversionRate = productUnit.ConversionRate;
            UnitPrice = productUnit.UnitPrice;
            IsBaseUnit= productUnit.IsBaseUnit;
        }
    }

}
