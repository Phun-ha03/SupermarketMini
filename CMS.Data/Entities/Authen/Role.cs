using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class Role : IdentityRole<int>
    {
        public string Description { set; get; }
        public string Controler { set; get; }
        public string Action { set; get; }
        public string Icon { set; get; }
        public short SortOrder { set; get; }
        public byte IsShow { set; get; }
        public int ParentRoleId { set; get; }
        public byte LevelId { set; get; }
        public byte StatusId { set; get; }

        public List<RoleFunction> RoleFunctions { get; set; }
    }
}
