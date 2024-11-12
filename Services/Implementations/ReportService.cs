using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TrendLine.Data;
using TrendLine.Enums;
using TrendLine.Models;
using TrendLine.Services.Interfaces;

namespace TrendLine.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Report> GenerateReport(ReportType reportType)
        {
            var report = new Report
            {
                ReportType = reportType,
                GeneratedAt = DateTime.UtcNow,
                ReportData = reportType switch
                {
                    ReportType.DailySales => await GenerateDailySalesReport(),
                    ReportType.MonthlySales => await GenerateMonthlySalesReport(),
                    ReportType.TopProducts => await GenerateTopProductsReport(),
                    _ => throw new ArgumentOutOfRangeException()
                }
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return report;
        }

        private async Task<string> GenerateDailySalesReport()
        {
            var today = DateTime.UtcNow.Date;
            var salesData = await _context.Orders
                .Where(o => o.OrderDate >= today && o.OrderDate < today.AddDays(1) && o.Status == "Completed")
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    TotalSales = g.Sum(oi => oi.Quantity * oi.Price)
                })
                .ToListAsync();

            return JsonConvert.SerializeObject(salesData);
        }

        private async Task<string> GenerateMonthlySalesReport()
        {
            var firstOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var salesData = await _context.Orders
                .Where(o => o.OrderDate >= firstOfMonth && o.OrderDate < firstOfMonth.AddMonths(1) && o.Status == "Completed")
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    TotalSales = g.Sum(oi => oi.Quantity * oi.Price)
                })
                .ToListAsync();

            return JsonConvert.SerializeObject(salesData);
        }

        private async Task<string> GenerateTopProductsReport()
        {
            var topProducts = await _context.OrderItems
                .GroupBy(oi => oi.ProductId)
                .OrderByDescending(g => g.Sum(oi => oi.Quantity))
                .Take(10)
                .Select(g => new
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    TotalSales = g.Sum(oi => oi.Quantity * oi.Price)
                })
                .ToListAsync();

            return JsonConvert.SerializeObject(topProducts);
        }
    }
}
