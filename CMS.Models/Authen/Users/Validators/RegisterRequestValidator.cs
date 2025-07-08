using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users.Validators
{
    public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Chưa nhập tên đăng nhập!")
                .MinimumLength(4).WithMessage("Tên đăng nhập quá ngắn!")
                .MaximumLength(250).WithMessage("Tên đăng nhập quá dài!");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Chưa nhập mật khẩu!")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự!");

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Chưa xác nhận mật khẩu!");

            RuleFor(x => x).Custom((request, context) =>
            {
                if (!string.IsNullOrEmpty(request.Password)
                    && !string.IsNullOrEmpty(request.ConfirmPassword)
                    && !request.Password.Equals(request.ConfirmPassword))
                {
                    context.AddFailure("Xác nhận mật khẩu không khớp!");
                }
            });

            RuleFor(x => x.FullName).NotEmpty().WithMessage("Chưa nhập Họ và Tên!")
                .MinimumLength(6).WithMessage("Họ và Tên quá ngắn!")
                .MaximumLength(250).WithMessage("Họ và Tên quá dài!");

            /*RuleFor(x => x.DateOfBirth).GreaterThan(DateTime.Now.AddYears(-100))
                .WithMessage("Ngày sinh không hợp lệ");*/

            /*RuleFor(x => x.Email).NotEmpty().WithMessage("Chưa nhập Email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email không hợp lệ");*/
        }
    }
}
