using CMS.Models.Supermarket.Homes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket.Interfaces
{
    public interface IHomeService
    {
        List<DailyRevenueReportViewModel> GetRevenueReport(DateTime fromDate, DateTime toDate, string filterType);
        List<BestSellingProductViewModel> GetBestSellingProducts(DateTime? fromDate = null, DateTime? toDate = null, int top = 10);
        List<ProductExpireSoonViewModel> GetProductsExpireSoon(int daysBeforeExpire = 30);
        List<ProductLowStockViewModel> GetLowStockProducts(int threshold = 10);
        DashboardReportViewModel GetDashboardReport(DateTime fromDate, DateTime toDate, string filterType);
    }
}
