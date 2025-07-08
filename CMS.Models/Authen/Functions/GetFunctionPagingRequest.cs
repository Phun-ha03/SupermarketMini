using CMS.BaseModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Functions
{
    public class GetFunctionPagingRequest:PagingRequestBase
    {
        [Display(Name = "Chức năng cha")]
        public int ParentRoleId { set; get; }
        [Display(Name = "Từ khóa")]
        public string Keyword { set; get; }
    }
}
