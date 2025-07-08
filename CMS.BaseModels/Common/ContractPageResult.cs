using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BaseModels.Common
{
    public class ContractPageResult<T>: PagedResultBase
    {
        public long TotalContractValue { set; get; }
        public long TotalPaidProfitValue { set; get; }
        public long TotalUntilTodayProfitValue { set; get; }

        public long AvgProfitPerDay { set; get; }
        public float AvgProfitPerMonth { set; get; }

        public List<T> Items { set; get; }

        public ContractPageResult()
        {
            Items = new List<T>();
        }
    }
}
