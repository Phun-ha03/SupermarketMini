using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Categories;
using CMS.Models.Supermarket.ProductUnits;
using CMS.Models.Supermarket.Suppliers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Products
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ProductCode { get;set; }
        public decimal Price { get; set; }
        public decimal StockQuantity { get; set; }
        public string Barcode { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? CategoryID { get; set; }
        public int? SupplierID { get; set; }
        public string CategoryName { get; set; }
        public string SupplierName { get; set; }
        public string? Image { get; set; }   
        public IFormFile ImageUpload { get; set; }
        public  ICollection<ProductUnitViewModel> ProductUnits { get; set; } = new List<ProductUnitViewModel>();


        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<SupplierViewModel> Suppliers { get; set; } = new List<SupplierViewModel>();

        public ProductViewModel() { }

        public ProductViewModel(Product product)
        {
            ProductID = product.ProductID;
            Name = product.Name;
            ProductCode= product.ProductCode;
            Description = product.Description;
            Price = product.Price;
            StockQuantity = product.StockQuantity;
            Barcode = product.Barcode;
            ExpirationDate = product.ExpirationDate;
            ProductCode= product.ProductCode;
            CategoryID = product.CategoryID;
            SupplierID = product.SupplierID;
            Image= product.Image;
            ProductUnits=new List<ProductUnitViewModel>();
          

        }
    }
}
