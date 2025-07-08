using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.RoleClaims.Validators
{
    public class RoleClaimCreateRequestValidator:AbstractValidator<RoleClaimCreateRequest>
    {
        public RoleClaimCreateRequestValidator()
        {
            RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("Id Quyền không hợp lệ");

            RuleFor(x => x.ClaimType).NotEmpty().WithMessage("Chưa nhập Loại");
            RuleFor(x => x.ClaimValue).NotEmpty().WithMessage("Chưa nhập Giá trị");
        }
    }
}
