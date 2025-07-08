using CMS.BaseModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Products
{
    public class GetProductPagingRequest: PagingRequestBase
    {

        [Display(Name = "Từ khóa")]
        public string ?Keyword { set; get; }
        public int? CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? Barcode { get; set; }
        public string? priceFilter { get; set; }
    }
}
