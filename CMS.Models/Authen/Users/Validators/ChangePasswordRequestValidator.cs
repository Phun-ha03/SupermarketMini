using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users.Validators
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Chưa nhập mật khẩu hiện tại");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Chưa nhập mật khẩu mới")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Chưa nhập lại mật khẩu mới");
            RuleFor(x => x).Custom((request, context) =>
            {
                if (!string.IsNullOrEmpty(request.NewPassword)
                    && !string.IsNullOrEmpty(request.ConfirmPassword)
                    && !request.NewPassword.Equals(request.ConfirmPassword))
                {
                    context.AddFailure("ConfirmPassword", "Xác nhận mật khẩu không khớp");
                }
            });
        }
    }
}
