using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Icons
{
    public class IconEditRequest
    {

        [Display(Name = "Id")]
        public int IconId { get; set; }

        [Display(Name = "Icon code")]
        public string IconCode { get; set; }

        [Display(Name = "Icon type")]
        public byte IconTypeId { get; set; }

        [Display(Name = "Icon type code")]
        public string IconTypeCode { get; set; }

        [Display(Name = "Status")]
        public byte StatusId { get; set; }

        public IconEditRequest() { }

        public IconEditRequest(IconViewModel Icon)
        {
            IconId = Icon.IconId;
            IconCode = Icon.IconCode;
            IconTypeId = Icon.IconTypeId;
            IconTypeCode = Icon.IconTypeCode;
            StatusId = Icon.StatusId;
        }
    }
}
