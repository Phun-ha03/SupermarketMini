using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.RoleClaims
{
    public class RoleClaimViewModel
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        [Display(Name = "Id Quyền")]
        public int RoleId { get; set; }
        [Display(Name = "Loại")]
        public string ClaimType { get; set; }
        [Display(Name = "Giá trị")]
        public string ClaimValue { get; set; }

        public RoleClaimViewModel() { }

        public RoleClaimViewModel(IdentityRoleClaim<int> identityRoleClaim)
        {
            Id = identityRoleClaim.Id;
            RoleId = identityRoleClaim.RoleId;
            ClaimType = identityRoleClaim.ClaimType;
            ClaimValue = identityRoleClaim.ClaimValue;
        }
    }
}
