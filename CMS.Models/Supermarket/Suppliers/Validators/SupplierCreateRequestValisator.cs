using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Suppliers.Validators
{
    public class SupplierCreateRequestValisator : AbstractValidator<SupplierCreateRequest>
    {
        public SupplierCreateRequestValisator()
        {
            RuleFor(x => x.SupplierName).NotEmpty().WithMessage("Chưa nhập Tên")
                .MaximumLength(150).WithMessage("Tên dài tối đa 150 ký tự");
        }
    }
}
