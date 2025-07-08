using CMS.Data.Entities.Supermarket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Categories
{
    public class CategoryEditRequest
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public CategoryEditRequest() { }
        public CategoryEditRequest(Category category)
        {
            CategoryID = category.CategoryID;
            CategoryName = category.CategoryName;
            Description = category.Description;
            CreateAt = category.CreateAt;
            UpdateAt = category.UpdateAt;
        }
        public CategoryEditRequest(CategoryViewModel category)
        {
            CategoryID = category.CategoryID;
            CategoryName = category.CategoryName;
            Description = category.Description;
            CreateAt = category.CreateAt;
            UpdateAt = category.UpdateAt;
        }
    }
}
