using TrendLine.Enums;

namespace TrendLine.DTOs
{
    public class AddProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Gender Gender { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
        public int? DiscountId { get; set; }
    }
}
