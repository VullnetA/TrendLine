namespace TrendLine.DTOs
{
    public class ProductSearchDTO
    {
        public string? Category { get; set; }
        public string? Gender { get; set; }
        public string? Brand { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public bool? InStock { get; set; }
    }
}
