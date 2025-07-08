using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class Gender
    {
        public int GenderId { set; get; }
        public string GenderName { set; get; }
        public string GenderDesc { set; get; }

        public List<User> AppUsers { set; get; }

    }
}
