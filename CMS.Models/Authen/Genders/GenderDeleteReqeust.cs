using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Genders
{
    public class GenderDeleteReqeust
    {
        [Display(Name = "Id")]
        public int GenderId { set; get; }
        [Display(Name = "Tên")]
        public string GenderName { set; get; }
        [Display(Name = "Mô tả")]
        public string GenderDesc { set; get; }
    }
}
