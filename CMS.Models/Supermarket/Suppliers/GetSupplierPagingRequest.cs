using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Suppliers
{
    public class GetSupplierPagingRequest
    {
        [Display(Name = "Từ khóa")]
        public string Keyword { set; get; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
