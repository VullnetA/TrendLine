using Microsoft.AspNetCore.Authorization;
using TrendLine.DTOs;
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
            try
            {
                var brand = await _catalogService.GetBrandById(input.BrandId);
                if (brand == null)
                {
                    throw CreateGraphQLError("Brand not found", "NOT_FOUND", $"Brand ID {input.BrandId} does not exist.");
                }

                var category = await _catalogService.GetCategoryById(input.CategoryId);
                if (category == null)
                {
                    throw CreateGraphQLError("Category not found", "NOT_FOUND", $"Category ID {input.CategoryId} does not exist.");
                }

                var color = await _catalogService.GetColorById(input.ColorId);
                if (color == null)
                {
                    throw CreateGraphQLError("Color not found", "NOT_FOUND", $"Color ID {input.ColorId} does not exist.");
                }

                var size = await _catalogService.GetSizeById(input.SizeId);
                if (size == null)
                {
                    throw CreateGraphQLError("Size not found", "NOT_FOUND", $"Size ID {input.SizeId} does not exist.");
                }

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
            catch (GraphQLException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw CreateGraphQLError("An unexpected error occurred", "INTERNAL_SERVER_ERROR", ex.Message);
            }
        }

        private async Task<double> CalculateDiscountedPriceAsync(double price, int discountId)
        {
            var discount = await _discountService.GetDiscountById(discountId);

            if (discount == null || !discount.DiscountPercentage.HasValue)
            {
                throw CreateGraphQLError("Invalid discount", "INVALID_INPUT", $"Discount ID {discountId} is invalid or missing.");
            }

            double discountPercentage = discount.DiscountPercentage.Value;
            return price - (price * discountPercentage / 100);
        }

        private GraphQLException CreateGraphQLError(string message, string code, string details)
        {
            var error = ErrorBuilder.New()
                .SetMessage(message)
                .SetCode(code)
                .SetExtension("details", details)
                .SetExtension("timestamp", DateTime.UtcNow.ToString("o"))
                .Build();

            return new GraphQLException(error);
        }
    }
}
