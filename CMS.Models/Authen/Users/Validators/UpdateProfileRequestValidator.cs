using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users.Validators
{
    public class UpdateProfileRequestValidator:AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0)
                .WithMessage("Id không hợp lệ");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Chưa nhập Họ và Tên")
                .MinimumLength(6).WithMessage("Họ và Tên quá ngắn")
                .MaximumLength(250).WithMessage("Họ và Tên quá dài");

            /*RuleFor(x => x.GenderId).GreaterThan(0)
                .WithMessage("Chưa chọn giới tính");*/

            /*RuleFor(x => x.DateOfBirth)
                .GreaterThan(DateTime.Now.AddYears(-100))
                .WithMessage("Ngày sinh không hợp lệ");*/

            /*RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Chưa nhập Email")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email không hợp lệ");*/

            /*RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Chưa nhập số điện thoại")
                .MinimumLength(9).WithMessage("Số điện thoại quá ngắn")
                .MaximumLength(12).WithMessage("Số điện thoại quá dài");*/

        }
    }
}
