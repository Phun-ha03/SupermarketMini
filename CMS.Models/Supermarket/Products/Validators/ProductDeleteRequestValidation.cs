using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Products.Validators
{
    public class ProductDeleteRequestValidation:AbstractValidator<ProductDeleteRequest>
    {
        public ProductDeleteRequestValidation()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("Id không xác định")
                 .GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
