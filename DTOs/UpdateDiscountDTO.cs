namespace TrendLine.DTOs
{
    public class UpdateDiscountDTO
    {
        public int Id { get; set; }
        public decimal? DiscountAmount { get; set; }
        public double? DiscountPercentage { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
