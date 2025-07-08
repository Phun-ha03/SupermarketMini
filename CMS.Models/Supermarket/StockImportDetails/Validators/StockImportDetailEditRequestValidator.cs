using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.StockImportDetails.Validators
{
    public class StockImportDetailEditRequestValidator : AbstractValidator<StockImportDetailEditRequest>
    {
        public StockImportDetailEditRequestValidator()
        {
            RuleFor(x => x.ImportDetailID).NotEmpty().WithMessage("Id không xác định")
                .GreaterThan(0).WithMessage("Id không hợp lệ");
        }
    }
}
