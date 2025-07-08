using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BaseModels.Common
{
    public class TransactionPageResult<T>: PagedResultBase
    {
        public long TotalIncomValue { set; get; }
        public long TotalOutcomValue { set; get; }
        public long TotalOrtherFee { set; get; }
        public List<T> Items { set; get; }

        public TransactionPageResult()
        {
            Items = new List<T>();
        }
    }
}
