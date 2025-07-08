using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class ForgotPassWordRequest
    {
        [Display(Name = "Email")]
        public string? Email { set; get; }
    }
}
