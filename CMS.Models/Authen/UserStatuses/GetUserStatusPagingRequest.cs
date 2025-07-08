using CMS.BaseModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserStatuses
{
    public class GetUserStatusPagingRequest: PagingRequestBase
    {
        [Display(Name = "Keyword")]
        public string Keyword { set; get; }
    }
}
