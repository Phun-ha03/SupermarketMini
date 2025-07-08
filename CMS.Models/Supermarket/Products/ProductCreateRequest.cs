using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.Suppliers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Products
{
    public class ProductCreateRequest
    {
        [Display(Name = "Tên loại")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public string? ProductCode { get; set; }

        [Display(Name = "Gia")]
        public decimal Price { get; set; }
        [Display(Name = "Mã vạch")]
        public string Barcode { get; set; }
        [Display(Name = "Số lượng trong kho")]
        public decimal StockQuantity { get; set; }

        [Display(Name = "Hạn")]
        public DateTime? ExpirationDate { get; set; }
        [Display(Name = "Loại sản phẩm")]
        public int? CategoryID { get; set; }
        [Display(Name = "Nhà cung cấp")]
        public int? SupplierID { get; set; }
        [Display(Name = "Ảnh")]
        public string? Image {  get; set; }
        public IFormFile? ImageUpload { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<SupplierViewModel> Suppliers { get; set; } = new List<SupplierViewModel>();

        public ProductCreateRequest() { }
    }
}
