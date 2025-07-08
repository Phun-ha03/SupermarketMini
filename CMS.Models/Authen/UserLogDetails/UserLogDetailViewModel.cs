using CMS.Data.Entities.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserLogDetails
{
    public class UserLogDetailViewModel
    {
        public int UserLogDetailId { get; set; }

        public int? UserLogId { get; set; }

        public string? OriginData { get; set; }

        public string? NewData { get; set; }

        public string? ActionMessage { get; set; }
        public UserLogDetailViewModel() { }
        public UserLogDetailViewModel(UserLogDetail userLogDetails)
        {
            UserLogDetailId = userLogDetails.UserLogDetailId;
            UserLogId = userLogDetails.UserLogId;
            OriginData = userLogDetails.OriginData;
            NewData = userLogDetails.NewData;
            ActionMessage = userLogDetails.ActionMessage;
        }
    }
}
