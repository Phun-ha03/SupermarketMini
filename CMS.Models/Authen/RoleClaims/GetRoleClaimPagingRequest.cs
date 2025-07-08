using CMS.BaseModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.RoleClaims
{
    public class GetRoleClaimPagingRequest: PagingRequestBase
    {
        [Display(Name = "Quyền")]
        public int RoleId { set; get; }
        [Display(Name = "Từ khóa")]
        public string Keyword { set; get; }
    }
}
