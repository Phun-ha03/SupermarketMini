using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.OrderDetails.Validators
{
    public class OrderDetailDeleteRequestValidation: AbstractValidator<OrderDetailDeleteRequest>
    {
        public OrderDetailDeleteRequestValidation()
        {
            RuleFor(x => x.OrderDetailID).NotEmpty().WithMessage("Id không xác định")
                .GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
