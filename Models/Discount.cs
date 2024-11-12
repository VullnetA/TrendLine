using System.ComponentModel.DataAnnotations;

namespace TrendLine.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } // Optional, e.g., "Holiday Sale"
        public decimal DiscountAmount { get; set; } // e.g., $10 off
        public double? DiscountPercentage { get; set; } // e.g., 20% off
        public DateTime? ExpirationDate { get; set; } // Optional, when the discount expires
    }
}
