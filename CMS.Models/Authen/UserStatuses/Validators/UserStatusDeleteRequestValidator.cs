using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserStatuses.Validators
{
    public class UserStatusDeleteRequestValidator: AbstractValidator<UserStatusDeleteRequest>
    {
        public UserStatusDeleteRequestValidator()
        {
            RuleFor(x => x.UserStatusId).GreaterThan((byte)0).WithMessage("Id không hợp lệ");
        }
    }
}
