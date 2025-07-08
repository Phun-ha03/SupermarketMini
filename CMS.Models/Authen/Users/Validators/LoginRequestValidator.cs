using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users.Validators
{
    public class LoginRequestValidator :AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Nhập vào tên đăng nhập!")
                .MinimumLength(4).WithMessage("Tên đăng nhập quá ngắn!")
                .MaximumLength(250).WithMessage("Tên đăng nhập quá dài! ");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Nhập vào mật khẩu!")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự!");
        }
    }
}
