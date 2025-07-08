using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class RoleFunction
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }

        public Role Role { get; set; }
        public Function Function { get; set; }
    }
}
