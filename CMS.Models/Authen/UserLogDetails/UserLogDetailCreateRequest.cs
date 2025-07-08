using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserLogDetails
{
    public class UserLogDetailCreateRequest
    {
        public string OriginData { get; set; } = string.Empty;

        public string NewData { get; set; } = string.Empty;

        public string ActionMessage { get; set; } = string.Empty;
        public UserLogDetailCreateRequest(string originData, string newData, string actionMessage)
        {
            OriginData = originData;
            NewData = newData;
            ActionMessage = actionMessage;
        }
    }
}
