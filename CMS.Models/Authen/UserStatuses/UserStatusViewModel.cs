using CMS.Data.Entities.Authen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserStatuses
{
    public class UserStatusViewModel
    {
        [Display(Name = "Id")]
        public byte UserStatusId { get; set; }

        [Display(Name = "Name")]
        public string UserStatusName { get; set; }

        [Display(Name = "Description")]
        public string UserStatusDesc { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }

        [Display(Name = "Background color")]
        public string BackgroundColor { get; set; }

        public UserStatusViewModel() { }

        public UserStatusViewModel(UserStatus userStatus)
        {
            UserStatusId = userStatus.UserStatusId;
            UserStatusName = userStatus.UserStatusName;
            UserStatusDesc = userStatus.UserStatusDesc;
            Color = userStatus.Color;
            BackgroundColor = userStatus.BackgroundColor;
        }
    }
}
