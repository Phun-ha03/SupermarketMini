using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserClaims
{
    public class UserClaimCreateRequest
    {

        [Display(Name = "Id Người dùng")]
        public int UserId { get; set; }
        [Display(Name = "Loại")]
        public string? ClaimType { get; set; }
        [Display(Name = "Giá trị")]
        public string? ClaimValue { get; set; }
    }
}
