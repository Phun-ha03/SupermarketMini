using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.OrderDetails.Validators
{
    public class OrderDetailCreateRequestValidation : AbstractValidator<OrderDetailCreateRequest>
    {
        public OrderDetailCreateRequestValidation()
        {
            RuleFor(x => x.ProductID).GreaterThan(0).WithMessage("Id Quyền không hợp lệ");

        }
    }
}
