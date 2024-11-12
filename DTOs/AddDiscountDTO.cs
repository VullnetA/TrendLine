namespace TrendLine.DTOs
{
    public class AddDiscountDTO
    {
        public string Name { get; set; }
        public decimal DiscountAmount { get; set; }
        public double? DiscountPercentage { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
