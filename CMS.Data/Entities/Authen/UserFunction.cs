using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class UserFunction
    {
        public int UserId { get; set; }
        public int FunctionId { get; set; }

        public User User { get; set; }
        public Function Function { get; set; }
    }
}
