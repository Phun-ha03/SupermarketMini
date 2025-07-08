using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BaseModels.Common
{
    public class PagedResult<T>: PagedResultBase
    {
        public List<T> Items { set; get; }

        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}
