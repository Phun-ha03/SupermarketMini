using CMS.Data.Entities.Authen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Icons
{
    public class IconViewModel
    {

        [Display(Name = "Id")]
        public int IconId { get; set; }

        [Display(Name = "Icon code")]
        public string IconCode { get; set; }

        [Display(Name = "Icon type")]
        public byte IconTypeId { get; set; }

        public string IconTypeCode { get; set; }

        [Display(Name = "Status")]
        public byte StatusId { get; set; }

        public IconViewModel() { }

        public IconViewModel(Icon Icon)
        {
            IconId = Icon.IconId;
            IconCode = Icon.IconCode;
            IconTypeId = Icon.IconTypeId;
            IconTypeCode = Icon.IconTypeCode;
            StatusId = Icon.StatusId;
        }
    }
}
