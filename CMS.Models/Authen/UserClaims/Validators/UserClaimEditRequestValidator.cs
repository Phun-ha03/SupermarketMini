using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserClaims.Validators
{
    public class UserClaimEditRequestValidator:AbstractValidator<UserClaimEditRequest>
    {
        public UserClaimEditRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id không hợp lệ");

            RuleFor(x => x.UserId).NotEmpty().WithMessage("Id Người dùng không hợp lệ");
            RuleFor(x => x.ClaimType).NotEmpty().WithMessage("Chưa nhập Loại");
            RuleFor(x => x.ClaimValue).NotEmpty().WithMessage("Chưa nhập Giá trị");
        }
    }
}
