using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Categories.Validators
{
    public class CategoryCreateRequestValisator : AbstractValidator<CategoryCreateRequest>
    {
        public CategoryCreateRequestValisator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Chưa nhập Tên")
                .MaximumLength(150).WithMessage("Tên dài tối đa 150 ký tự");
        }
    }
}
