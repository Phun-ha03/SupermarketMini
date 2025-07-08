using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Customers.Validators
{
    public class CustomerCreateRequestValisator : AbstractValidator<CustomerCreateRequest>
    {
        public CustomerCreateRequestValisator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Chưa nhập Tên")
                .MaximumLength(150).WithMessage("Tên dài tối đa 150 ký tự");
        }
    }
}
