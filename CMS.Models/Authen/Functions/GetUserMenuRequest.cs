using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Functions
{
    public class GetUserMenuRequest
    {
        [Display(Name = "Người dùng")]
        public int UserId { set; get; }
    }
}
