using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class ResetPasswordRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Token")]
        public string? Token { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string? ConfirmPassword { get; set; }

        public ResetPasswordRequest() { }

        public ResetPasswordRequest(UserViewModel user)
        {
            Id = user.Id;
        }
    }
}
