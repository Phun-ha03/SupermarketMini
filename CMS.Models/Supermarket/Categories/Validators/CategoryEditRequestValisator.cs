using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Categories.Validators
{
    public class CategoryEditRequestValisator : AbstractValidator<ProductEditRequest>
    {
        public CategoryEditRequestValisator()
        {
            RuleFor(x => x.ProductID).NotEmpty().WithMessage("Id không xác định")
                .GreaterThan(0).WithMessage("Id không hợp lệ");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Chưa nhập Tên")
                .MaximumLength(150).WithMessage("Tên dài tối đa 150 ký tự");
        }
    
    }
}
