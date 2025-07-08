using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Icons
{
    public class IconCreateRequest
    {

        [Display(Name = "Icon code")]
        public string IconCode { get; set; }

        [Display(Name = "Icon type")]
        public byte IconTypeId { get; set; }

        [Display(Name = "Icon type code")]
        public string IconTypeCode { get; set; }

        [Display(Name = "Status")]
        public byte StatusId { get; set; }

        public IconCreateRequest() { }
    }
}
