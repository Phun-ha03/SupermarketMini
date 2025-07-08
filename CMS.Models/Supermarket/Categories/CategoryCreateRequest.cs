using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Categories
{
    public class CategoryCreateRequest
    {

        public int CategotyID { get; set; }
        [Display(Name = "Tên danh mục hàng")]
        public string CategoryName { get; set; }
        [Display(Name = "Mô tả danh mục")]
        public string Description { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreateAt { get; set; }
        [Display(Name = "Ngày cập nhật")]
        public DateTime? UpdateAt { get; set; }
        public CategoryCreateRequest() { }
    }
}
