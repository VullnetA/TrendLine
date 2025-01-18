using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TrendLine.Data;
using TrendLine.Enums;
using TrendLine.Models;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Handles report generation for TrendLine application.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="reportService">The report service to handle report generation.</param>
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Generates a daily sales report.
        /// </summary>
        /// <remarks>
        /// Available in version 1.0.
        /// </remarks>
        /// <returns>The daily sales report.</returns>
        /// <response code="200">Returns the daily sales report.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet("daily-sales")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Report>> GenerateDailySalesReport()
        {
            var report = await _reportService.GenerateReport(ReportType.DailySales);
            return Ok(report);
        }

        /// <summary>
        /// Generates a monthly sales report.
        /// </summary>
        /// <remarks>
        /// Available in version 1.0.
        /// </remarks>
        /// <returns>The monthly sales report.</returns>
        /// <response code="200">Returns the monthly sales report.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet("monthly-sales")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Report>> GenerateMonthlySalesReport()
        {
            var report = await _reportService.GenerateReport(ReportType.MonthlySales);
            return Ok(report);
        }

        /// <summary>
        /// Generates a report of the top products.
        /// </summary>
        /// <remarks>
        /// Available in version 1.0.
        /// </remarks>
        /// <returns>The report of top products.</returns>
        /// <response code="200">Returns the top products report.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet("top-products")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Report>> GenerateTopProductsReport()
        {
            var report = await _reportService.GenerateReport(ReportType.TopProducts);
            return Ok(report);
        }
    }
}
