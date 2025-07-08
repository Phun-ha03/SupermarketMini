using System;
using System.Collections.Generic;
using CMS.Models.Supermarket.StockImportDetails;
using CMS.Models.Supermarket.Suppliers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMS.Models.Supermarket.StockImports
{
    public class StockImportCreateRequest
    {
        public int SupplierID { get; set; }
        public decimal? TotalCost { get; set; } 
        public DateTime? ImportDate { get; set; }
        public List<StockImportDetailCreateRequest> StockImportDetails { get; set; } = new List<StockImportDetailCreateRequest>();
        public decimal? DiscountAmount { get; set; } = 0;
      
        public StockImportCreateRequest() { }
    }
}
