using Microsoft.AspNetCore.Authorization;
using TrendLine.Data;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Implementations;
using TrendLine.Services.Interfaces;

namespace TrendLine.GraphQL
{
    public class Mutation
    {
        private readonly IProductService _productService;
        private readonly ICatalogService _catalogService;
        private readonly IDiscountService _discountService;

        public Mutation(IProductService productService,
                        ICatalogService catalogService,
                        IDiscountService discountService)
        {
            _productService = productService;
            _catalogService = catalogService;
            _discountService = discountService;
        }

        [Authorize(Roles = "Admin, Advanced User")]
        [GraphQLDescription("Adds a new product.")]
        public async Task<ProductDTO> AddProduct(AddProductDTO input)
        {
            var brand = await _catalogService.GetBrandById(input.BrandId);
            var category = await _catalogService.GetCategoryById(input.CategoryId);
            var color = await _catalogService.GetColorById(input.ColorId);
            var size = await _catalogService.GetSizeById(input.SizeId);

            await _productService.AddProduct(input);

            double finalPrice = input.Price;
            if (input.DiscountId.HasValue)
            {
                finalPrice = await CalculateDiscountedPriceAsync(input.Price, input.DiscountId.Value);
            }

            return new ProductDTO
            {
                Name = input.Name,
                Description = input.Description,
                Price = input.Price,
                FinalPrice = finalPrice,
                Quantity = input.Quantity,
                Gender = input.Gender.ToString(),
                Brand = brand.Name,
                Category = category.Name,
                Color = color.Name,
                Size = size.Label,
            };
        }

        private async Task<double> CalculateDiscountedPriceAsync(double price, int discountId)
        {
            var discount = await _discountService.GetDiscountById(discountId);

            if (discount == null || !discount.DiscountPercentage.HasValue)
            {
                throw new InvalidOperationException("Invalid or missing discount percentage.");
            }

            double discountPercentage = discount.DiscountPercentage.Value;
            return price - (price * discountPercentage / 100);
        }
    }
}
