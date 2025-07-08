using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Icons.Validators
{
    public class IconEditRequestValidator : AbstractValidator<IconEditRequest>
    {
        public IconEditRequestValidator()
        {
            RuleFor(x => x.IconId).GreaterThan(0).WithMessage("Id không hợp lệ");

            RuleFor(x => x.IconCode).NotEmpty().WithMessage("Chưa nhập IconCode");

            RuleFor(x => x.IconTypeId).NotEmpty().WithMessage("Chưa chọn loại icon").
                GreaterThan((byte)0).WithMessage("Loại icon không hợp lệ");

            RuleFor(x => x.IconTypeCode).NotEmpty().WithMessage("Chưa nhập icon type code");
        }
    }
}
