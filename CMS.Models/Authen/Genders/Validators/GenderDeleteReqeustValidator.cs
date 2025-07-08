using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Genders.Validators
{
    public class GenderDeleteReqeustValidator: AbstractValidator<GenderDeleteReqeust>
    {
        public GenderDeleteReqeustValidator()
        {
            RuleFor(x => x.GenderId).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
