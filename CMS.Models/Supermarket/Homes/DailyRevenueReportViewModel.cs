using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Homes
{
    public class DailyRevenueReportViewModel
    {
        public DateTime Date { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
    }
}
