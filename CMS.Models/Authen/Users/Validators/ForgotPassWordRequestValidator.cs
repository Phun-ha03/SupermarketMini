using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users.Validators
{
    public class ForgotPassWordRequestValidator:AbstractValidator<ForgotPassWordRequest>
    {
        public ForgotPassWordRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Chưa nhập Email!")
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email không hợp lệ!");
        }
    }
}
