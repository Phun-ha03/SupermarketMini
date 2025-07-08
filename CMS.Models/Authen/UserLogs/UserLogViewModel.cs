using CMS.Data.Entities.Authen;
using CMS.Models.Authen.UserLogDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserLogs
{
    public class UserLogViewModel
    {
        public int UserLogId { get; set; }

        public int? UserId { get; set; }

        public string? IpAddress { get; set; }

        public int? ActionId { get; set; }

        public string? TableName { get; set; }

        public long? TableRowId { get; set; }

        public DateTime? CrDateTime { get; set; }

        public byte? StatusId { get; set; }

        public virtual List<UserLogDetailViewModel> UserLogDetails { get; set; } = new List<UserLogDetailViewModel>();

        public UserLogViewModel()
        {
        }
        public UserLogViewModel(UserLog userLog)
        {
            UserLogId = userLog.UserLogId;
            UserId = userLog.UserId;
            IpAddress = userLog.IpAddress;
            ActionId = userLog.ActionId;
            TableName = userLog.TableName;
            TableRowId = userLog.TableRowId;
            CrDateTime = userLog.CrDateTime;
            StatusId = userLog.StatusId;
            if (userLog.UserLogDetails != null)
            {
                foreach (var item in userLog.UserLogDetails)
                {
                    var detail = new UserLogDetailViewModel(item);
                    UserLogDetails.Add(detail);
                }
            }
        }
    }
}
