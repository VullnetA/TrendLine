using TrendLine.Enums;

namespace TrendLine.DTOs
{
    public class EditProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Gender Gender { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int ColorId { get; set; }
        public int SizeId { get; set; }
    }
}
