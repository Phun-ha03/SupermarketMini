using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ProductUnits
{
    public class ProductUnitDeleteRequest
    {
        public int UnitID { get; set; }
        public int? ProductID { get; set; }
        public string UnitName { get; set; }
        public ProductUnitDeleteRequest() { }
        public ProductUnitDeleteRequest(ProductUnit productUnit)
        {
            UnitID = productUnit.UnitID;
            ProductID = productUnit.ProductID;
            UnitName = productUnit.UnitName;
        }
        public ProductUnitDeleteRequest(ProductUnitViewModel productUnit)
        {
            UnitID= productUnit.UnitID;
            ProductID = productUnit.ProductID;
            UnitName = productUnit.UnitName;
        }
    }
}
