using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        // Brand Endpoints
        [HttpGet("brands")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var response = await _catalogService.GetAllBrands();
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("brands/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BrandDTO>> GetBrandById(int id)
        {
            var response = await _catalogService.GetBrandById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost("brands")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddBrand(BrandDTO brand)
        {
            await _catalogService.AddBrand(brand);
            return Ok();
        }

        [HttpPut("brands/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateBrand(BrandDTO brand, int id)
        {
            await _catalogService.UpdateBrand(brand, id);
            return Ok();
        }

        [HttpDelete("brands/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            await _catalogService.DeleteBrand(id);
            return Ok();
        }

        // Category Endpoints
        [HttpGet("categories")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var response = await _catalogService.GetAllCategories();
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var response = await _catalogService.GetCategoryById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost("categories")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddCategory(CategoryDTO category)
        {
            await _catalogService.AddCategory(category);
            return Ok();
        }

        [HttpPut("categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateCategory(CategoryDTO category, int id)
        {
            await _catalogService.UpdateCategory(category, id);
            return Ok();
        }

        [HttpDelete("categories/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _catalogService.DeleteCategory(id);
            return Ok();
        }

        // Color Endpoints
        [HttpGet("colors")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetAllColors()
        {
            var response = await _catalogService.GetAllColors();
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("colors/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ColorDTO>> GetColorById(int id)
        {
            var response = await _catalogService.GetColorById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost("colors")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddColor(ColorDTO color)
        {
            await _catalogService.AddColor(color);
            return Ok();
        }

        [HttpPut("colors/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateColor(ColorDTO color, int id)
        {
            await _catalogService.UpdateColor(color, id);
            return Ok();
        }

        [HttpDelete("colors/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteColor(int id)
        {
            await _catalogService.DeleteColor(id);
            return Ok();
        }

        // Size Endpoints
        [HttpGet("sizes")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SizeDTO>>> GetAllSizes()
        {
            var response = await _catalogService.GetAllSizes();
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("sizes/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SizeDTO>> GetSizeById(int id)
        {
            var response = await _catalogService.GetSizeById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost("sizes")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddSize(SizeDTO size)
        {
            await _catalogService.AddSize(size);
            return Ok();
        }

        [HttpPut("sizes/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateSize(SizeDTO size, int id)
        {
            await _catalogService.UpdateSize(size, id);
            return Ok();
        }

        [HttpDelete("sizes/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSize(int id)
        {
            await _catalogService.DeleteSize(id);
            return Ok();
        }
    }
}
