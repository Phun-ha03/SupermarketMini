using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserClaims.Validators
{
    public class UserClaimDeleteRequestValidator: AbstractValidator<UserClaimDeleteRequest>
    {
        public UserClaimDeleteRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
