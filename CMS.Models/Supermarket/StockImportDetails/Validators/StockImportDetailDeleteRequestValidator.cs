using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImportDetails.Validators
{
    internal class StockImportDetailDeleteRequestValidator : AbstractValidator<StockImportDetailEditRequest>
    {
        public StockImportDetailDeleteRequestValidator()
        {
            RuleFor(x => x.ImportDetailID).NotEmpty().WithMessage("Id không xác định")
                .GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
