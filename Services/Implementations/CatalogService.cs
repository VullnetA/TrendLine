using AutoMapper;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Repositories.Implementations;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Interfaces;

namespace TrendLine.Services.Implementations
{
    public class CatalogService : ICatalogService
    {
        private readonly IMapper _mapper;
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository, IMapper mapper)
        {
            _catalogRepository = catalogRepository;
            _mapper = mapper;
        }

        // Brand CRUD operations
        public async Task<IEnumerable<BrandDTO>> GetAllBrands()
        {
            var brands = await _catalogRepository.GetAllBrands();
            return brands?.Select(brand => _mapper.Map<BrandDTO>(brand))
                   ?? Enumerable.Empty<BrandDTO>();
        }

        public async Task<BrandDTO> GetBrandById(int id)
        {
            var brand = await _catalogRepository.GetBrandById(id);
            return brand != null ? _mapper.Map<BrandDTO>(brand) : null;
        }

        public async Task AddBrand(BrandDTO brand)
        {
            await _catalogRepository.AddBrand(brand);
        }

        public async Task UpdateBrand(BrandDTO brand, int id)
        {
            await _catalogRepository.UpdateBrand(brand, id);
        }

        public async Task DeleteBrand(int id)
        {
            await _catalogRepository.DeleteBrand(id);
        }

        // Category CRUD operations
        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var categories = await _catalogRepository.GetAllCategories();
            return categories?.Select(category => _mapper.Map<CategoryDTO>(category))
                   ?? Enumerable.Empty<CategoryDTO>();
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _catalogRepository.GetCategoryById(id);
            return category != null ? _mapper.Map<CategoryDTO>(category) : null;
        }

        public async Task AddCategory(CategoryDTO category)
        {
            await _catalogRepository.AddCategory(category);
        }

        public async Task UpdateCategory(CategoryDTO category, int id)
        {
            await _catalogRepository.UpdateCategory(category, id);
        }

        public async Task DeleteCategory(int id)
        {
            await _catalogRepository.DeleteCategory(id);
        }

        // Color CRUD operations
        public async Task<IEnumerable<ColorDTO>> GetAllColors()
        {
            var colors = await _catalogRepository.GetAllColors();
            return colors?.Select(color => _mapper.Map<ColorDTO>(color))
                   ?? Enumerable.Empty<ColorDTO>();
        }

        public async Task<ColorDTO> GetColorById(int id)
        {
            var color = await _catalogRepository.GetColorById(id);
            return color != null ? _mapper.Map<ColorDTO>(color) : null;
        }

        public async Task AddColor(ColorDTO color)
        {
            await _catalogRepository.AddColor(color);
        }

        public async Task UpdateColor(ColorDTO color, int id)
        {
            await _catalogRepository.UpdateColor(color, id);
        }

        public async Task DeleteColor(int id)
        {
            await _catalogRepository.DeleteColor(id);
        }

        // Size CRUD operations
        public async Task<IEnumerable<SizeDTO>> GetAllSizes()
        {
            var sizes = await _catalogRepository.GetAllSizes();
            return sizes?.Select(size => _mapper.Map<SizeDTO>(size))
                   ?? Enumerable.Empty<SizeDTO>();
        }

        public async Task<SizeDTO> GetSizeById(int id)
        {
            var size = await _catalogRepository.GetSizeById(id);
            return size != null ? _mapper.Map<SizeDTO>(size) : null;
        }

        public async Task AddSize(SizeDTO size)
        {
            await _catalogRepository.AddSize(size);
        }

        public async Task UpdateSize(SizeDTO size, int id)
        {
            await _catalogRepository.UpdateSize(size, id);
        }

        public async Task DeleteSize(int id)
        {
            await _catalogRepository.DeleteSize(id);
        }
    }
}
