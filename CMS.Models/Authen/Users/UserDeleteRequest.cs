using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class UserDeleteRequest
    {
        [Display(Name = "Id")]
        public int Id { set; get; }
        [Display(Name = "Tải khoản")]
        public string UserName { set; get; }
        [Display(Name = "Họ và Tên")]
        public string FullName { set; get; }
        public bool Deleteable { set; get; }
    }
}
