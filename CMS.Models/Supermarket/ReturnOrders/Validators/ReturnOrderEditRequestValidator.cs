using CMS.Models.Supermarket.Orders;
using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ReturnOrders.Validators
{
    public class ReturnOrderEditRequestValidator : AbstractValidator<OrderEditRequest>
    {
        public ReturnOrderEditRequestValidator()
        {
            RuleFor(x => x.OrderID).NotEmpty().WithMessage("Id không xác định")
                .GreaterThan(0).WithMessage("Id không hợp lệ");
            RuleFor(x => x.CustomerID).NotEmpty().WithMessage("Id không xác định")
                .GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
    
}
