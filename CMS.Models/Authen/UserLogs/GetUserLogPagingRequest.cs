using CMS.BaseModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserLogs
{
    public class GetUserLogPagingRequest:   PagingRequestBase
    {

        [Display(Name = "Từ khóa")]
        public string Keyword { set; get; }
        public int? ActionId { get; set; }
        public int? StatusId { get; set; }
    }
}
