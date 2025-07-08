using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Customers.Validators
{
    public class CustomerDeleteRequestValisator : AbstractValidator<CustomerDeleteRequest>
    {
        public CustomerDeleteRequestValisator()
        {
            RuleFor(x => x.CustomerID).NotEmpty().WithMessage("Id không xác định")
                 .GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
