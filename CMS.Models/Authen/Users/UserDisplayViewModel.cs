using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Authen.Users
{
    public class UserDisplayViewModel
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string FullName { get; set; }
        public string RoleName { get; set; }
        public string StatusName { get; set; }

        public string Avatar { get; set; }

        public string Code { get; set; }

        public string Background { get; set; }
    }
}
