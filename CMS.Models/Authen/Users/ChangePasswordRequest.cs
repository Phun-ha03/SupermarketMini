using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class ChangePasswordRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu hiện tại")]
        public string? OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        public string? ConfirmPassword { get; set; }

        public ChangePasswordRequest() { }

        public ChangePasswordRequest(UserViewModel user)
        {
            Id = user.Id;
        }
    }
}
