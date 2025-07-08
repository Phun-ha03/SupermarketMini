using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Categories
{
    public class CategoryDeleteRequest
    {
        public int CategoryID { get; set; }
        [Display(Name = "Tên danh mục hàng")]
        public string CategoryName { get; set; }
        [Display(Name = "Mô tả danh mục")]
        public string Description { get; set; }
        
        public CategoryDeleteRequest() { }
        
        public CategoryDeleteRequest(Category category)
        {
            CategoryID = category.CategoryID;
            CategoryName = category.CategoryName;
            Description = category.Description;
           

        }

        public CategoryDeleteRequest(CategoryViewModel category)
        {
            CategoryID= category.CategoryID;
            CategoryName = category.CategoryName;
            Description = category.Description;
           
        }
    }
}
