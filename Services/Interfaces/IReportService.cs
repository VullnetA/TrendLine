using TrendLine.Enums;
using TrendLine.Models;

namespace TrendLine.Services.Interfaces
{
    public interface IReportService
    {
        Task<Report> GenerateReport(ReportType reportType);
    }
}
