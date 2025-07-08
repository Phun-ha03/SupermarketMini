using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Functions.Validators
{
    public class FunctionDeleteRequestValidator: AbstractValidator<FunctionDeleteRequest>
    {
        public FunctionDeleteRequestValidator()
        {
            RuleFor(x => x.FunctionId).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
