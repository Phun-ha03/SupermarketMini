using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class LoginRequest
    {
        [Display(Name = "Tài khoản")]
        public string? UserName { set; get; }
        [Display(Name = "Mật khẩu")]
        public string? Password { set; get; }
        [Display(Name = "Ghi nhớ")]
        public bool RememberMe { set; get; }
        [Display(Name = "Trở lại")]
        public string? ReturnUrl { set; get; }
    }
}
