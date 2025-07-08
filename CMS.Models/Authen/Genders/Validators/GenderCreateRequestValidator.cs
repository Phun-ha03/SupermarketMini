using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Genders.Validators
{
    public class GenderCreateRequestValidator: AbstractValidator<GenderCreateRequest>
    {
        public GenderCreateRequestValidator()
        {
            RuleFor(x => x.GenderName).NotEmpty().WithMessage("Chưa nhập tên");
            RuleFor(x => x.GenderDesc).NotEmpty().WithMessage("Chưa nhập mô tả");
        }
    }
}
