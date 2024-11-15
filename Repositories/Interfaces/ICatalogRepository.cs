﻿using TrendLine.DTOs;
using TrendLine.Models;

namespace TrendLine.Repositories.Interfaces
{
    public interface ICatalogRepository
    {
        // Brand CRUD operations
        Task<IEnumerable<Brand>> GetAllBrands();
        Task<Brand> GetBrandById(int id);
        Task AddBrand(BrandDTO brand);
        Task UpdateBrand(BrandDTO brand, int id);
        Task DeleteBrand(int id);

        // Category CRUD operations
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task AddCategory(CategoryDTO category);
        Task UpdateCategory(CategoryDTO category, int id);
        Task DeleteCategory(int id);

        // Color CRUD operations
        Task<IEnumerable<Color>> GetAllColors();
        Task<Color> GetColorById(int id);
        Task AddColor(ColorDTO color);
        Task UpdateColor(ColorDTO color, int id);
        Task DeleteColor(int id);

        // Size CRUD operations
        Task<IEnumerable<Size>> GetAllSizes();
        Task<Size> GetSizeById(int id);
        Task AddSize(SizeDTO size);
        Task UpdateSize(SizeDTO size, int id);
        Task DeleteSize(int id);
    }
}
