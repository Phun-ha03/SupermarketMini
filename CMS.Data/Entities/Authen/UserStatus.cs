using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class UserStatus
    {
        public byte UserStatusId { get; set; }
        public string UserStatusName { get; set; }
        public string UserStatusDesc { get; set; }
        public string Color { get; set; }
        public string BackgroundColor { get; set; }

        public List<User> AppUsers { set; get; }
    }
}
