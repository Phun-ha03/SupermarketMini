using CMS.BaseModels.Common;
using CMS.Data.EF;
using CMS.Data.Entities.Supermarket;
using CMS.Data.Entities.Supermarket;
using CMS.Models.Supermarket.Homes;
using CMS.Models.Supermarket.Homes;
using CMS.Services.Supermarket.Interfaces;
using CMS.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Supermarket

{
    public class HomeService : IHomeService
    {
        private readonly AICMSDBContext _context;

        public HomeService(AICMSDBContext context
        )
        {
            _context = context;
        }

        public List<DailyRevenueReportViewModel> GetRevenueReport(DateTime fromDate, DateTime toDate, string filterType)
        {
            var orders = _context.Orders
                .Where(o => o.CreateAt.Date >= fromDate.Date && o.CreateAt.Date <= toDate.Date)
                .AsEnumerable(); // ⚠️ chuyển sang xử lý ở client

            IEnumerable<DailyRevenueReportViewModel> result;

            switch (filterType?.ToLower())
            {
                case "month":
                    result = orders
                        .GroupBy(o => new { o.CreateAt.Year, o.CreateAt.Month })
                        .Select(g => new DailyRevenueReportViewModel
                        {
                            Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                            TotalRevenue = g.Sum(x => x.TotalAmount - x.Discount),
                            OrderCount = g.Count()
                        });
                    break;

                case "year":
                    result = orders
                        .GroupBy(o => o.CreateAt.Year)
                        .Select(g => new DailyRevenueReportViewModel
                        {
                            Date = new DateTime(g.Key, 1, 1),
                            TotalRevenue = g.Sum(x => x.TotalAmount - x.Discount),
                            OrderCount = g.Count()
                        });
                    break;

                case "week":
                    result = orders
                        .GroupBy(o => System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                            o.CreateAt, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday))
                        .Select(g => new DailyRevenueReportViewModel
                        {
                            Date = g.Min(x => x.CreateAt).Date,
                            TotalRevenue = g.Sum(x => x.TotalAmount - x.Discount),
                            OrderCount = g.Count()
                        });
                    break;

                default: // day
                    result = orders
                        .GroupBy(o => o.CreateAt.Date)
                        .Select(g => new DailyRevenueReportViewModel
                        {
                            Date = g.Key,
                            TotalRevenue = g.Sum(x => x.TotalAmount - x.Discount),
                            OrderCount = g.Count()
                        });
                    break;
            }

            return result.OrderBy(x => x.Date).ToList();
        }
        public List<BestSellingProductViewModel> GetBestSellingProducts(DateTime? fromDate = null, DateTime? toDate = null, int top = 10)
        {
            var query = from od in _context.OrderDetails
                        join o in _context.Orders on od.OrderID equals o.OrderID
                        join p in _context.Products on od.ProductID equals p.ProductID
                        join u in _context.ProductUnits
                            on new { ProductID = (int?)od.ProductID, od.UnitID }
                            equals new { u.ProductID, u.UnitID }
                        select new
                        {
                            od.ProductID,
                            p.Name,
                            QuantityBase = od.Quantity * u.ConversionRate,
                            o.CreateAt
                        };


            if (fromDate.HasValue)
                query = query.Where(x => x.CreateAt >= fromDate.Value.Date);
            if (toDate.HasValue)
                query = query.Where(x => x.CreateAt <= toDate.Value.Date);

            var result = query
     .GroupBy(x => new { x.ProductID, x.Name })
     .Select(g => new BestSellingProductViewModel
     {
         ProductID = g.Key.ProductID,
         ProductName = g.Key.Name,
         TotalQuantitySold = (int)g.Sum(x => x.QuantityBase)
     })
     .OrderByDescending(x => x.TotalQuantitySold)
     .Take(top)
     .ToList();


            return result;
        }

        public List<ProductExpireSoonViewModel> GetProductsExpireSoon(int daysBeforeExpire = 30)
        {
            var thresholdDate = DateTime.Today.AddDays(daysBeforeExpire);

            var result = _context.StockImportDetails
                .Where(x =>
                    x.ExpirationDate != null &&
                    x.ExpirationDate > DateTime.Today &&                     
                    x.ExpirationDate <= thresholdDate &&
                    x.Quantity > x.UsedQuantity                              
                )
                .Select(x => new ProductExpireSoonViewModel
                {
                    ProductID = x.ProductID,
                    ProductName = x.Product.Name,
                    ExpirationDate = x.ExpirationDate.Value,
                    StockQuantity = (x.Quantity - x.UsedQuantity) *
                            _context.ProductUnits
                                .Where(pu => pu.ProductID == x.ProductID && pu.UnitID == x.UnitID)
                                .Select(pu => pu.ConversionRate)
                                .FirstOrDefault()
                })
                .OrderBy(x => x.ExpirationDate)
                .ToList();

            return result;
        }

        public List<ProductLowStockViewModel> GetLowStockProducts(int threshold = 10)
        {
            var result = _context.Products
                .Where(p => p.StockQuantity <= threshold)
                .Select(p => new ProductLowStockViewModel
                {
                    ProductName = p.Name,
                    StockQuantity = (int)p.StockQuantity
                })
                .OrderBy(p => p.StockQuantity)
                .ToList();

            return result;
        }
        public List<UserRevenueViewModel> GetUserRevenues(DateTime fromDate, DateTime toDate, int? userId = null)
        {
            var query = from o in _context.Orders
                        where o.CreateAt >= fromDate && o.CreateAt <= toDate
                              && o.CreatedBy != null
                        join u in _context.Users on o.CreatedBy equals u.Id into gj
                        from user in gj.DefaultIfEmpty()
                        select new { o, u = user };

            if (userId.HasValue)
                query = query.Where(x => x.u != null && x.u.Id == userId.Value);

            var grouped = query
                .GroupBy(x => new { Id = x.u.Id, Name = x.u.FullName })
                .Select(g => new UserRevenueViewModel
                {
                    UserID = g.Key.Id,
                    UserName = g.Key.Name ?? "Không xác định",
                    TotalOrders = g.Count(),
                    TotalRevenue = g.Sum(x => x.o.TotalAmount - x.o.Discount)
                });

            return grouped.ToList();
        }


        public DashboardReportViewModel GetDashboardReport(DateTime fromDate, DateTime toDate, string filterType)
        {
            return new DashboardReportViewModel
            {
                FromDate = fromDate,
                ToDate = toDate,
                FilterType = filterType,
                RevenueReports = GetRevenueReport(fromDate, toDate, filterType),
                BestSellingProducts = GetBestSellingProducts(fromDate, toDate),
                ProductsExpireSoon = GetProductsExpireSoon(),
                ProductsLowStock = GetLowStockProducts(),
                UserRevenues = GetUserRevenues(fromDate, toDate)
            };
        }

    }
}
