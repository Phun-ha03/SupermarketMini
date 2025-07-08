using CMS.Data.Entities.Authen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserFunctions
{
    public class UserFunctionViewModel
    {

        [Display(Name = "User")]
        public int UserId { get; set; }
        [Display(Name = "Function")]
        public int FunctionId { get; set; }

        public UserFunctionViewModel(UserFunction userFunction)
        {
            UserId = userFunction.UserId;
            FunctionId = userFunction.FunctionId;
        }
    }
}
