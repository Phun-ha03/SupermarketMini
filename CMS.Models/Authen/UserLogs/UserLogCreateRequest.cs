using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.UserLogs
{
    public class UserLogCreateRequest
    {
        public int? UserId { get; set; }

        public string? IpAddress { get; set; }

        public int? ActionId { get; set; }

        public string? TableName { get; set; }

        public long? TableRowId { get; set; }

        public DateTime? CrDateTime { get; set; }

        public byte? StatusId { get; set; }
        //public ICollection<UserLogDetail> UserLogDetails { get; set; } = new List<UserLogDetail>();
    }
}

