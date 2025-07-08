using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Orders.Validators
{
    public class OrderDeleteRequestValidation : AbstractValidator<OrderDeleteRequest>
    {
        public OrderDeleteRequestValidation()
        {
            RuleFor(x => x.OrderID).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
