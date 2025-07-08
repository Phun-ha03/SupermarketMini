using CMS.Data.Entities.Supermarket;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Products
{
    public class ProductEditRequest
    {
        public int ProductID { get; set; }
        [Display(Name = "Tên loại ")]
        public string Name { get; set; }
        [Display(Name = "Mã sản phẩm")]
        public string? ProductCode { get; set; }
        [Display(Name = "mÔ Tả sản phẩm")]
        public string Description { get; set; }
        [Display(Name = "Giá")]
        public decimal Price { get; set; }
        [Display(Name = "Tồn kho")]
        public decimal StockQuantity { get; set; }
        [Display(Name = "Mã vạch")]
        public string Barcode { get; set; }
        public int? CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? Image {  get; set; }
        public IFormFile? ImageUpload { get; set; }
        public ProductEditRequest() { }

        public ProductEditRequest(Product product)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            Description = product.Description;
            ProductCode=product.ProductCode;
            Price = product.Price;
            StockQuantity = product.StockQuantity;
            Barcode = product.Barcode;
            ExpirationDate = product.ExpirationDate;
            CategoryID = product.CategoryID;
            SupplierID = product.SupplierID;
            Image=product.Image;
            
        }

        public ProductEditRequest(ProductViewModel product)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            Description = product.Description;
            ProductCode = product.ProductCode;
            Price = product.Price;
            StockQuantity = product.StockQuantity;
            Barcode = product.Barcode;
            ExpirationDate = product.ExpirationDate;
            CategoryID = product.CategoryID;
            SupplierID = product.SupplierID;
            Image=product.Image;
        }
    }
}
