using CMS.Models.Supermarket.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Suppliers.Validators
{
    public class SupplierDeleteRequestValisator : AbstractValidator<SupplierDeleteRequest>
    {
        public SupplierDeleteRequestValisator()
        {
            RuleFor(x => x.SupplierID).GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
