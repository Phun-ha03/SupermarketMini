using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ReturnOrders.Validators
{
    public class ReturnOrderDeleteRequestValisdtor : AbstractValidator<ReturnOrderDeleteRequest>
    {
        public ReturnOrderDeleteRequestValisdtor()
        {
            RuleFor(x => x.ReturnID).NotEmpty().WithMessage("Id không xác định")
                 .GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
