using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Roles.Validators
{
    public class RoleCreateRequestValidator:AbstractValidator<RoleCreateRequest>
    {
        public RoleCreateRequestValidator()
        {
            RuleFor(x => x.SortOrder).GreaterThan((short)0).WithMessage("Chưa nhập Thứ tự");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Chưa nhập Tên");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Chưa nhập Mô tả");
        }
    }
}
