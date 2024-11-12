using TrendLine.DTOs;

namespace TrendLine.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProducts();
        Task<ProductDTO> GetProductById(int id);
        Task AddProduct(AddProductDTO productDto);
        Task UpdateProduct(EditProductDTO productDto, int id);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductDTO>> FindByCategory(string category);
        Task<IEnumerable<ProductDTO>> FindByBrand(string brand);
        Task<IEnumerable<ProductDTO>> FindByGender(string gender);
        Task<IEnumerable<ProductDTO>> FindByPriceRange(double minPrice, double maxPrice);
        Task<IEnumerable<ProductDTO>> FindBySize(string size);
        Task<IEnumerable<ProductDTO>> FindByColor(string color);
        Task<long> CountByCategory(string category);
        Task<long> CountByBrand(string brand);
        Task<long> CountByAvailability();
        Task<long> CountOutOfStock();
        Task UpdateQuantity(int productId, int quantity);
        Task<ProductQuantityDTO> GetProductQuantity(int productId);
        Task<IEnumerable<ProductDTO>> SearchProducts(ProductSearchDTO searchParams);
    }
}
