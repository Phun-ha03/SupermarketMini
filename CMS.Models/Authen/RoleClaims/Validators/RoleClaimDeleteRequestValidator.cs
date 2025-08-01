﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.RoleClaims.Validators
{
    public class RoleClaimDeleteRequestValidator :AbstractValidator<RoleClaimDeleteRequest>
    {
        public RoleClaimDeleteRequestValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
