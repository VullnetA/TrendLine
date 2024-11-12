using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrendLine.Data;
using TrendLine.Enums;
using TrendLine.Models;
using TrendLine.Services.Implementations;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("daily-sales")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<Report>> GenerateDailySalesReport()
        {
            var report = await _reportService.GenerateReport(ReportType.DailySales);
            return Ok(report);
        }

        [HttpGet("monthly-sales")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<Report>> GenerateMonthlySalesReport()
        {
            var report = await _reportService.GenerateReport(ReportType.MonthlySales);
            return Ok(report);
        }

        [HttpGet("top-products")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<Report>> GenerateTopProductsReport()
        {
            var report = await _reportService.GenerateReport(ReportType.TopProducts);
            return Ok(report);
        }
    }
}
