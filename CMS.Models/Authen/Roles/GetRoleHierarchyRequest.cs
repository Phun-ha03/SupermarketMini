using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Roles
{
    public class GetRoleHierarchyRequest
    {
        [Display(Name = "Người dùng")]
        public int UserId { set; get; }
        [Display(Name = "Quyền")]
        public int RoleGroupId { set; get; }
        [Display(Name = "Chức năng cha")]
        public int ParentRoleId { set; get; }
        [Display(Name = "Từ khóa")]
        public string Keyword { set; get; }
        [Display(Name = "Hiển thị")]
        public byte IsShow { set; get; }
        [Display(Name = "Trạng thái")]
        public byte StatusId { set; get; }

        public GetRoleHierarchyRequest()
        {
            UserId = RoleGroupId = ParentRoleId = 0;
            IsShow = StatusId = 2;
            Keyword = string.Empty;
        }
    }
}
