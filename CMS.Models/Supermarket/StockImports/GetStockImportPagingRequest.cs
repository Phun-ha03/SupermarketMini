using CMS.BaseModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImports
{
    public class GetStockImportPagingRequest : PagingRequestBase
    {
        [Display(Name = "Từ khóa")]
        public string? Keyword { set; get; }
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; }
    }
}
