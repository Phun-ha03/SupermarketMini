using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.RoleClaims
{
    public class RoleClaimEditRequest
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Id Quyền")]
        public int RoleId { get; set; }
        [Display(Name = "Loại")]
        public string? ClaimType { get; set; }
        [Display(Name = "Giá trị")]
        public string? ClaimValue { get; set; }

        public RoleClaimEditRequest() { }

        public RoleClaimEditRequest(RoleClaimViewModel roleClaimViewModel)
        {
            Id = roleClaimViewModel.Id;
            RoleId = roleClaimViewModel.RoleId;
            ClaimType = roleClaimViewModel.ClaimType;
            ClaimValue = roleClaimViewModel.ClaimValue;
        }
    }
}
