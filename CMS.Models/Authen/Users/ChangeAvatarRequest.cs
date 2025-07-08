using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class ChangeAvatarRequest
    {

        [Display(Name = "Tài khoản")]
        public int Id { set; get; }
        [Display(Name = "Avatar")]
        public string Avatar { set; get; }
    }
}
