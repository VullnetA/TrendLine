using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.Enums;
using TrendLine.Models;
using TrendLine.Services.Helpers;
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
        /// <returns>The daily sales report.</returns>
        /// <response code="200">Returns the daily sales report.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Report not found.</response>
        [HttpGet("daily-sales")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Report>> GenerateDailySalesReport()
        {
            try
            {
                var report = await _reportService.GenerateReport(ReportType.DailySales);
                if (report == null)
                {
                    return ErrorHandler.NotFoundResponse(this, "Daily sales report not found.");
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return ErrorHandler.InternalServerErrorResponse(this, "Error generating daily sales report.", ex);
            }
        }

        /// <summary>
        /// Generates a monthly sales report.
        /// </summary>
        /// <returns>The monthly sales report.</returns>
        /// <response code="200">Returns the monthly sales report.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Report not found.</response>
        [HttpGet("monthly-sales")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Report>> GenerateMonthlySalesReport()
        {
            try
            {
                var report = await _reportService.GenerateReport(ReportType.MonthlySales);
                if (report == null)
                {
                    return ErrorHandler.NotFoundResponse(this, "Monthly sales report not found.");
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return ErrorHandler.InternalServerErrorResponse(this, "Error generating monthly sales report.", ex);
            }
        }

        /// <summary>
        /// Generates a report of the top products.
        /// </summary>
        /// <returns>The report of top products.</returns>
        /// <response code="200">Returns the top products report.</response>
        /// <response code="401">Unauthorized access.</response>
        /// <response code="404">Report not found.</response>
        [HttpGet("top-products")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(Report), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Report>> GenerateTopProductsReport()
        {
            try
            {
                var report = await _reportService.GenerateReport(ReportType.TopProducts);
                if (report == null)
                {
                    return ErrorHandler.NotFoundResponse(this, "Top products report not found.");
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return ErrorHandler.InternalServerErrorResponse(this, "Error generating top products report.", ex);
            }
        }
    }
}
