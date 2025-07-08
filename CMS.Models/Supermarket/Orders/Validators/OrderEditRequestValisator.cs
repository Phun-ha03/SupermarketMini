using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Orders.Validators
{
    public class OrderEditRequestValisator : AbstractValidator<OrderEditRequest>
    {
        public OrderEditRequestValisator()
        {
            RuleFor(x => x.OrderID).NotEmpty().WithMessage("Id không xác định")
                .GreaterThan(0).WithMessage("Id không hợp lệ");
            RuleFor(x => x.CustomerID).NotEmpty().WithMessage("Chưa nhập Tên")
                .GreaterThan(0).WithMessage("ID ko hợp lệ");
        }

    }
}
