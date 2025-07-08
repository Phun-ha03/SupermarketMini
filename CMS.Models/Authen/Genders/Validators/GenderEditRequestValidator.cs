using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Genders.Validators
{
    public class GenderEditRequestValidator:AbstractValidator<GenderEditRequest>
    {
        public GenderEditRequestValidator()
        {
            RuleFor(x => x.GenderId).GreaterThan(0).WithMessage("Id không hợp lệ");
            RuleFor(x => x.GenderName).NotEmpty().WithMessage("Chưa nhập tên");
            RuleFor(x => x.GenderDesc).NotEmpty().WithMessage("Chưa nhập mô tả");
        }
    }
}
