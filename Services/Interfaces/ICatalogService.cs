using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.Services.Interfaces
{
    public interface ICatalogService
    {
        // Brand CRUD operations
        Task<IEnumerable<BrandDTO>> GetAllBrands();
        Task<BrandDTO> GetBrandById(int id);
        Task AddBrand(BrandDTO brand);
        Task UpdateBrand(BrandDTO brand,int id);
        Task DeleteBrand(int id);

        // Category CRUD operations
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<CategoryDTO> GetCategoryById(int id);
        Task AddCategory(CategoryDTO category);
        Task UpdateCategory(CategoryDTO category, int id);
        Task DeleteCategory(int id);

        // Color CRUD operations
        Task<IEnumerable<ColorDTO>> GetAllColors();
        Task<ColorDTO> GetColorById(int id);
        Task AddColor(ColorDTO color);
        Task UpdateColor(ColorDTO color, int id);
        Task DeleteColor(int id);

        // Size CRUD operations
        Task<IEnumerable<SizeDTO>> GetAllSizes();
        Task<SizeDTO> GetSizeById(int id);
        Task AddSize(SizeDTO size);
        Task UpdateSize(SizeDTO size, int id);
        Task DeleteSize(int id);
    }
}
