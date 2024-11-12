using Microsoft.EntityFrameworkCore;
using TrendLine.Data;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;

namespace TrendLine.Repositories.Implementations
{
    public class CatalogRepository: ICatalogRepository
    {
        private readonly AppDbContext _context;

        public CatalogRepository(AppDbContext context)
        {
            _context = context;
        }

        // Brand CRUD operations
        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandById(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task AddBrand(BrandDTO brand)
        {
            Brand requestBody = new Brand();
            requestBody.Name = brand.Name;
            _context.Brands.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBrand(BrandDTO edit, int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                brand.Name = edit.Name;

                _context.Entry(brand).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteBrand(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }
        }

        // Category CRUD operations
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddCategory(CategoryDTO category)
        {
            var requestBody = new Category
            {
                Name = category.Name
            };
            _context.Categories.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategory(CategoryDTO edit, int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.Name = edit.Name;
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        // Color CRUD operations
        public async Task<IEnumerable<Color>> GetAllColors()
        {
            return await _context.Colors.ToListAsync();
        }

        public async Task<Color> GetColorById(int id)
        {
            return await _context.Colors.FindAsync(id);
        }

        public async Task AddColor(ColorDTO color)
        {
            var requestBody = new Color
            {
                Name = color.Name
            };
            _context.Colors.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateColor(ColorDTO edit, int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color != null)
            {
                color.Name = edit.Name;
                _context.Entry(color).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteColor(int id)
        {
            var color = await _context.Colors.FindAsync(id);
            if (color != null)
            {
                _context.Colors.Remove(color);
                await _context.SaveChangesAsync();
            }
        }

        // Size CRUD operations
        public async Task<IEnumerable<Size>> GetAllSizes()
        {
            return await _context.Sizes.ToListAsync();
        }

        public async Task<Size> GetSizeById(int id)
        {
            return await _context.Sizes.FindAsync(id);
        }

        public async Task AddSize(SizeDTO size)
        {
            var requestBody = new Size
            {
                Label = size.Label
            };
            _context.Sizes.Add(requestBody);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSize(SizeDTO edit, int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                size.Label = edit.Label;
                _context.Entry(size).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteSize(int id)
        {
            var size = await _context.Sizes.FindAsync(id);
            if (size != null)
            {
                _context.Sizes.Remove(size);
                await _context.SaveChangesAsync();
            }
        }
    }
}
