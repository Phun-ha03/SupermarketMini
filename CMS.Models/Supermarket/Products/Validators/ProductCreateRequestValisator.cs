using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Products.Validators
{
    public class ProductCreateRequestValisator : AbstractValidator<ProductCreateRequest>
    {
        public ProductCreateRequestValisator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Chưa nhập Tên")
                .MaximumLength(150).WithMessage("Tên dài tối đa 150 ký tự");
            RuleFor(x => x.ProductCode).NotEmpty().WithMessage("Chưa nhập Tên")
                .MaximumLength(150).WithMessage("Tên dài tối đa 150 ký tự");

        }
    }
}
