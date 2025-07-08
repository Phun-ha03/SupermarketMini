using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ProductUnits
{
    public class ProductUnitCreateRequest
    {
        
        [Display(Name = "Tên sản Phẩm")]
        public int? ProductID { get; set; }
        //[Display(Name = "Mã sản phẩm")]
        //public string ProductCode { get; set; }

        [Display(Name = "Đơn vị")]
        public string UnitName { get; set; }
        [Display(Name = "Số lượng/đơn vị")]
        public decimal ConversionRate { get; set; }
        [Display(Name = "Giá ")]
        public Decimal? UnitPrice { get; set; }
        [Display(Name = "Đơn vị cơ sở")]
        public bool IsBaseUnit { get; set; }
        public ProductUnitCreateRequest() { }
        public ProductUnitCreateRequest(ProductUnitViewModel productUnit)
        {
           
            ProductID = productUnit.ProductID;
            UnitName = productUnit.UnitName;
            ConversionRate = productUnit.ConversionRate;
            UnitPrice = productUnit.UnitPrice;
            IsBaseUnit = productUnit.IsBaseUnit;
        }

    }
}
