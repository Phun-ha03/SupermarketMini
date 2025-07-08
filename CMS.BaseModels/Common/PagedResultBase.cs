using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BaseModels.Common
{
    public class PagedResultBase
    {
        public int PageIndex { set; get; }
        public int PageSize { set; get; }
        public int TotalRecords { set; get; }
        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set { }
        }
    }
}
