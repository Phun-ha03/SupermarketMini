﻿using CMS.BaseModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.ReturnOrders
{
    public class GetReturnOrderPagingRequest : PagingRequestBase
    {
        [Display(Name = "Từ khóa")]
        public string? Keyword { set; get; }
    }
}
