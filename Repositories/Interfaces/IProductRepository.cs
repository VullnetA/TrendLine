using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(AddProductDTO productDto);
        Task UpdateProduct(EditProductDTO productDto, int id);
        Task DeleteProduct(int id);
        Task<IEnumerable<Product>> FindByCategory(string category);
        Task<IEnumerable<Product>> FindByBrand(string brand);
        Task<IEnumerable<Product>> FindByGender(string gender);
        Task<IEnumerable<Product>> FindByPriceRange(double minPrice, double maxPrice);
        Task<IEnumerable<Product>> FindBySize(string size);
        Task<IEnumerable<Product>> FindByColor(string color);
        Task<long> CountByCategory(string category);
        Task<long> CountByBrand(string brand);
        Task<long> CountByAvailability();
        Task<long> CountOutOfStock();
        Task UpdateQuantity(int productId, int quantity);
        Task<ProductQuantityDTO> GetProductQuantity(int productId);
        Task<IEnumerable<Product>> SearchProducts(ProductSearchDTO searchParams);
    }
}