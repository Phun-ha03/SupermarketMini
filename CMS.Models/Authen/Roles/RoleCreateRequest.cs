using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Roles
{
    public class RoleCreateRequest
    {
        [Display(Name = "Tên")]
        public string? Name { get; set; }
        [Display(Name = "Mô tả")]
        public string? Description { set; get; }
        [Display(Name = "Controller")]
        public string? Controler { set; get; }
        [Display(Name = "Action")]
        public string? Action { set; get; }
        [Display(Name = "Icon")]
        public string? Icon { set; get; }
        [Display(Name = "Thứ tự")]
        public short SortOrder { set; get; }
        [Display(Name = "Hiển thị")]
        public byte IsShow { set; get; }
        [Display(Name = "Thuộc Quyền")]
        public int ParentRoleId { set; get; }
        [Display(Name = "Level")]
        public byte LevelId { set; get; }
        [Display(Name = "Trạng thái")]
        public byte StatusId { set; get; }

        public bool IsShowBoolean { set; get; }
    }
}
