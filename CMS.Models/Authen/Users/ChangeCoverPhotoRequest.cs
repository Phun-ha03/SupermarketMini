using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class ChangeCoverPhotoRequest
    {
        [Display(Name = "Tài khoản")]
        public int Id { set; get; }
        [Display(Name = "Cover photo")]
        public string CoverPhoto { set; get; }
    }
}
