using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserStatuses.Validators
{
    public class UserStatusCreateRequestValidator : AbstractValidator<UserStatusCreateRequest>
    {
        public UserStatusCreateRequestValidator()
        {
            RuleFor(x => x.UserStatusId).GreaterThan((byte)0).WithMessage("Id không hợp lệ");
            RuleFor(x => x.UserStatusName).NotEmpty().WithMessage("Chưa nhập tên");
            RuleFor(x => x.Color).NotEmpty().WithMessage("Chưa nhập mầu chữ");
            RuleFor(x => x.BackgroundColor).NotEmpty().WithMessage("Chưa nhập mầu nền");
        }
    }
}
