using CMS.Models.Authen.Roles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class UserAssignRoleRequest
    {

        [Display(Name = "Tài khoản")]
        public int UserId { set; get; }
        [Display(Name = "Họ và Tên")]
        public string Fullname { set; get; }
        [Display(Name = "Quyền")]
        public List<RoleViewModel> Roles { set; get; }
    }
}
