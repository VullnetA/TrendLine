using TrendLine.Data;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Interfaces;

namespace TrendLine.GraphQL
{
    public class Mutation
    {
        private readonly IProductService _productService;
        private readonly ICatalogService _catalogService;
  

        public Mutation(IProductService productService,
                        ICatalogService catalogService)
        {
            _productService = productService;
            _catalogService = catalogService;
        }

        public async Task<ProductDTO> AddProduct(AddProductDTO input)
        {
            // Fetch related entities
            var brand = await _catalogService.GetBrandById(input.BrandId);
            var category = await _catalogService.GetCategoryById(input.CategoryId);
            var color = await _catalogService.GetColorById(input.ColorId);
            var size = await _catalogService.GetSizeById(input.SizeId);

            // Add the product
            await _productService.AddProduct(input);

            // Map to ProductDTO
            return new ProductDTO
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                FinalPrice = input.DiscountId.HasValue
                    ? CalculateDiscountedPrice(input.Price, input.DiscountId.Value)
                    : input.Price,
                Quantity = input.Quantity,
                Gender = input.Gender.ToString(),
                Brand = brand.Name,
                Category = category.Name,
                Color = color.Name,
                Size = size.Label,
            };
        }

        private double CalculateDiscountedPrice(double price, int discountId)
        {
            // Assume this logic calculates the discounted price
            // You can fetch the discount value using a DiscountService
            // Example: 10% discount
            double discountPercentage = 10; // Replace with actual logic
            return price - (price * discountPercentage / 100);
        }
    }
}