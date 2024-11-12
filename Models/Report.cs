using System.ComponentModel.DataAnnotations;
using TrendLine.Enums;

namespace TrendLine.Models
{
    public class Report
    {
        [Key]
        public int Id { get; set; }
        public ReportType ReportType { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string ReportData { get; set; }
    }
}
