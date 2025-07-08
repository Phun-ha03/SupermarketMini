using CMS.Models.Authen.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Roles
{
    public class RoleAssignFunctionRequest
    {
        [Display(Name = "Mã Quyền")]
        public int RoleId { get; set; }
        [Display(Name = "Tên Quyền")]
        public string? RoleName { get; set; }
        [Display(Name = "Chức năng")]
        public List<FunctionViewModel> Functions { get; set; }
    }
}
