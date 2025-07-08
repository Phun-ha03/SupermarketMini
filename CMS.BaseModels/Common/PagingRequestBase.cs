using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BaseModels.Common
{
    public class PagingRequestBase
    {
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
    }
}
