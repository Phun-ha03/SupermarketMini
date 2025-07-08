using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserClaims.Validators
{
    public class UserClaimCreateRequestValidator: AbstractValidator<UserClaimCreateRequest>
    {
        public UserClaimCreateRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Id Người dùng không hợp lệ");
            RuleFor(x => x.ClaimType).NotEmpty().WithMessage("Chưa nhập Loại");
            RuleFor(x => x.ClaimValue).NotEmpty().WithMessage("Chưa nhập Giá trị");
        }
    }
}
