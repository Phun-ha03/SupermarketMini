using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Icons.Validators
{
    public class IconDeleteRequestValidator:AbstractValidator<IconDeleteRequest>
    {
        public IconDeleteRequestValidator()
        {
            RuleFor(x => x.IconId).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
