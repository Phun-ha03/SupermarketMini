using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Supermarket.Homes
{
    public class DashboardReportViewModel
    {
        public List<DailyRevenueReportViewModel> RevenueReports { get; set; }
        public List<BestSellingProductViewModel> BestSellingProducts { get; set; }
        public List<ProductExpireSoonViewModel> ProductsExpireSoon { get; set; }
        public List<ProductLowStockViewModel> ProductsLowStock { get; set; }
        public List<UserRevenueViewModel> UserRevenues { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FilterType { get; set; }
    }
}
