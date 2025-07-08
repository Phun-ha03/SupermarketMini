using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Icons
{
    public class IconDeleteRequest
    {
        [Display(Name = "Id")]
        public int IconId { get; set; }

        [Display(Name = "Icon code")]
        public string IconCode { get; set; }

        [Display(Name = "Icon type")]
        public byte IconTypeId { get; set; }

        public string IconTypeCode { get; set; }

        public IconDeleteRequest() { }

        public IconDeleteRequest(IconViewModel Icon)
        {
            IconId = Icon.IconId;
            IconCode = Icon.IconCode;
            IconTypeId = Icon.IconTypeId;
            IconTypeCode = Icon.IconTypeCode;
        }
    }
}
