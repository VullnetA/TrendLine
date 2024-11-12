using System.ComponentModel.DataAnnotations;
using System.Drawing;
using TrendLine.Enums;

namespace TrendLine.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public Gender Gender { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int ColorId { get; set; }
        public Color Color { get; set; }

        public int SizeId { get; set; }
        public Size Size { get; set; }

        public int? DiscountId { get; set; }
        public Discount Discount { get; set; }

        public double GetFinalPrice()
        {
            if (Discount == null || (Discount.ExpirationDate.HasValue && Discount.ExpirationDate < DateTime.UtcNow))
            {
                return Price; // No discount or expired discount
            }

            // Apply discount based on percentage or fixed amount
            if (Discount.DiscountPercentage.HasValue)
            {
                return Price * (1 - (Discount.DiscountPercentage.Value / 100));
            }
            else if (Discount.DiscountAmount > 0)
            {
                return Price - (double)Discount.DiscountAmount;
            }

            return Price;
        }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
