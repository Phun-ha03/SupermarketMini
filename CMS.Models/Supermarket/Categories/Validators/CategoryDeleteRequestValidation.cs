using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Categories.Validators
{
    public class CategoryDeleteRequestValidation : AbstractValidator<CategoryDeleteRequest>
    {
        public CategoryDeleteRequestValidation()
        {
            RuleFor(x => x.CategoryID).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
