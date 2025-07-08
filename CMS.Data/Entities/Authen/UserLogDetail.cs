using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public partial class UserLogDetail
    {
        public int UserLogDetailId { get; set; }

        public int? UserLogId { get; set; }

        public string? OriginData { get; set; }

        public string? NewData { get; set; }

        public string? ActionMessage { get; set; }

        public virtual UserLog? UserLog { get; set; }
    }
}
