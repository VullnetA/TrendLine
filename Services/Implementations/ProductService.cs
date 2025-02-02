using AutoMapper;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Interfaces;
using HotChocolate.Subscriptions;

namespace TrendLine.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ITopicEventSender _eventSender;

        public ProductService(IMapper mapper, IProductRepository productRepository, ITopicEventSender eventSender)
        {
            _mapper = mapper;
            _productRepository = productRepository;
            _eventSender = eventSender;
        }

        public async Task AddProduct(AddProductDTO productDto)
        {
            await _productRepository.AddProduct(productDto);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();

            return products.Select(product =>
            {
                var productDto = _mapper.Map<ProductDTO>(product, opts => opts.Items["IsSingleProduct"] = false);

                productDto.FinalPrice = CalculateFinalPrice(product);

                return productDto;
            });
        }

        public async Task<ProductDTO> GetProductById(int id)
        {
            var product = await _productRepository.GetProductById(id);
            if (product == null) return null;

            var productDto = _mapper.Map<ProductDTO>(product, opts => opts.Items["IsSingleProduct"] = true);
            productDto.FinalPrice = CalculateFinalPrice(product);

            return productDto;
        }

        public async Task UpdateProduct(EditProductDTO productDto, int id)
        {
            await _productRepository.UpdateProduct(productDto, id);
        }

        public async Task<IEnumerable<ProductDTO>> FindByCategory(string category)
        {
            var products = await _productRepository.FindByCategory(category);
            return products?.Select(product => _mapper.Map<ProductDTO>(product))
                   ?? Enumerable.Empty<ProductDTO>();
        }

        public async Task<IEnumerable<ProductDTO>> FindByBrand(string brand)
        {
            var products = await _productRepository.FindByBrand(brand);
            return products?.Select(product => _mapper.Map<ProductDTO>(product))
                   ?? Enumerable.Empty<ProductDTO>();
        }

        public async Task<IEnumerable<ProductDTO>> FindByGender(string gender)
        {
            var products = await _productRepository.FindByGender(gender);
            return products?.Select(product => _mapper.Map<ProductDTO>(product))
                    ?? Enumerable.Empty<ProductDTO>();
        }

        public async Task<IEnumerable<ProductDTO>> FindByPriceRange(double minPrice, double maxPrice)
        {
            var products = await _productRepository.FindByPriceRange(minPrice, maxPrice);
            return products?.Select(product => _mapper.Map<ProductDTO>(product))
                    ?? Enumerable.Empty<ProductDTO>();
        }

        public async Task<IEnumerable<ProductDTO>> FindBySize(string size)
        {
            var products = await _productRepository.FindBySize(size);
            return products?.Select(product => _mapper.Map<ProductDTO>(product))
                    ?? Enumerable.Empty<ProductDTO>();
        }

        public async Task<IEnumerable<ProductDTO>> FindByColor(string color)
        {
            var products = await _productRepository.FindByColor(color);
            return products?.Select(product => _mapper.Map<ProductDTO>(product))
                   ?? Enumerable.Empty<ProductDTO>();
        }

        public async Task<long> CountByCategory(string category)
        {
            return await _productRepository.CountByCategory(category);
        }

        public async Task<long> CountByBrand(string brand)
        {
            return await _productRepository.CountByBrand(brand);
        }

        public async Task<long> CountByAvailability()
        {
            return await _productRepository.CountByAvailability();
        }

        public async Task<long> CountOutOfStock()
        {
            return await _productRepository.CountOutOfStock();
        }

        public async Task UpdateQuantity(int productId, int quantity)
        {
            await _productRepository.UpdateQuantity(productId, quantity);

            // Publish stock change event
            var updatedProduct = await GetProductById(productId);
            await _eventSender.SendAsync("ProductStockUpdated", updatedProduct);
        }

        public async Task<ProductQuantityDTO> GetProductQuantity(int productId)
        {
            return await _productRepository.GetProductQuantity(productId);
        }

        public async Task<IEnumerable<ProductDTO>> SearchProducts(ProductSearchDTO searchParams)
        {
            var products = await _productRepository.SearchProducts(searchParams);

            return products.Select(product =>
            {
                var productDto = _mapper.Map<ProductDTO>(product);
                productDto.FinalPrice = CalculateFinalPrice(product);
                return productDto;
            });
        }

        private double CalculateFinalPrice(Product product)
        {
            if (product.Discount == null || (product.Discount.ExpirationDate.HasValue && product.Discount.ExpirationDate < DateTime.UtcNow))
            {
                return product.Price;
            }

            if (product.Discount.DiscountPercentage.HasValue)
            {
                return product.Price * (1 - (product.Discount.DiscountPercentage.Value / 100));
            }
            else if (product.Discount.DiscountAmount > 0)
            {
                return product.Price - (double)product.Discount.DiscountAmount;
            }

            return product.Price;
        }
    }
}
