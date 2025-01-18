using AutoMapper;
using TrendLine.Repositories.Interfaces;
using TrendLine.DTOs;
using TrendLine.Services.Interfaces;

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
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            return await _productService.GetAllProducts();
        }
    }
}
