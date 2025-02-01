using TrendLine.DTOs;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Interfaces;

namespace TrendLine.GraphQL.Resolvers
{
    public class ProductResolvers
    {
        private readonly IProductRepository _productRepository;

        public ProductResolvers(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<string> GetBrand([Parent] ProductDTO product, [Service] ICatalogService catalogService)
        {
            var fullProduct = await _productRepository.GetProductById(product.Id);
            if (fullProduct == null) return "Unknown Brand";

            var brand = await catalogService.GetBrandById(fullProduct.BrandId);
            return brand?.Name ?? "Unknown Brand";
        }

        public async Task<string> GetCategory([Parent] ProductDTO product, [Service] ICatalogService catalogService)
        {
            var fullProduct = await _productRepository.GetProductById(product.Id);
            if (fullProduct == null) return "Unknown Category";

            var category = await catalogService.GetCategoryById(fullProduct.CategoryId);
            return category?.Name ?? "Unknown Category";
        }

        public async Task<string> GetColor([Parent] ProductDTO product, [Service] ICatalogService catalogService)
        {
            var fullProduct = await _productRepository.GetProductById(product.Id);
            if (fullProduct == null) return "Unknown Color";

            var color = await catalogService.GetColorById(fullProduct.ColorId);
            return color?.Name ?? "Unknown Color";
        }

        public async Task<string> GetSize([Parent] ProductDTO product, [Service] ICatalogService catalogService)
        {
            var fullProduct = await _productRepository.GetProductById(product.Id);
            if (fullProduct == null) return "Unknown Size";

            var size = await catalogService.GetSizeById(fullProduct.SizeId);
            return size?.Label ?? "Unknown Size";
        }
    }
}
