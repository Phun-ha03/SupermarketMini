using CMS.Data.Entities.Authen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.RoleFunctions
{
    public class RoleFunctionViewModel
    {

        [Display(Name = "Role")]
        public int RoleId { get; set; }
        [Display(Name = "Function")]
        public int FunctionId { get; set; }

        public RoleFunctionViewModel(RoleFunction roleFunction)
        {
            RoleId = roleFunction.RoleId;
            FunctionId = roleFunction.FunctionId;
        }
    }
}
