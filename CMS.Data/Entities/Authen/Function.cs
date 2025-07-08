using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Authen
{
    public class Function
    {
        public int FunctionId { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public string Controler { set; get; }
        public string Action { set; get; }
        public string Icon { set; get; }
        public short SortOrder { set; get; }
        public byte IsShow { set; get; }
        public int ParentFunctionId { set; get; }
        public byte LevelId { set; get; }
        public DateTime CrDateTime { set; get; }
        public byte StatusId { set; get; }

        public List<RoleFunction> RoleFunctions { set; get; }
        public List<UserFunction> UserFunctions { set; get; }
    }
}
