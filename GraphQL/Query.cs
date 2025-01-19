using TrendLine.DTOs;
using TrendLine.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace TrendLine.GraphQL
{
    public class Query
    {
        private readonly IProductService _productService;

        public Query(IProductService productService)
        {
            _productService = productService;
        }

        [GraphQLName("getProducts")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [GraphQLDescription("Fetches a list of all available products.")]
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            return await _productService.GetAllProducts();
        }
    }
}
