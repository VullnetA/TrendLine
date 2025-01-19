using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Helpers;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Manages catalog-related operations, including brands, categories, colors, and sizes.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogController"/> class.
        /// </summary>
        /// <param name="catalogService">Service for managing catalog data.</param>
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Retrieves all brands.
        /// </summary>
        /// <returns>A list of all brands.</returns>
        /// <response code="200">Returns the list of brands.</response>
        /// <response code="404">No brands found.</response>
        [HttpGet("brands")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<BrandDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var response = await _catalogService.GetAllBrands();
            if (response == null || !response.Any())
                return ErrorHandler.NotFoundResponse(this, "No brands found");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a brand by ID.
        /// </summary>
        /// <param name="id">The ID of the brand.</param>
        /// <returns>The requested brand.</returns>
        /// <response code="200">Returns the requested brand.</response>
        /// <response code="404">Brand not found.</response>
        [HttpGet("brands/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BrandDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BrandDTO>> GetBrandById(int id)
        {
            var response = await _catalogService.GetBrandById(id);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"Brand with ID {id} not found");

            return Ok(response);
        }

        /// <summary>
        /// Adds a new brand.
        /// </summary>
        /// <param name="brand">The details of the brand to add.</param>
        /// <returns>Status of the creation operation.</returns>
        /// <response code="200">Brand added successfully.</response>
        /// <response code="400">Invalid request or null input.</response>
        [HttpPost("brands")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddBrand(BrandDTO brand)
        {
            if (brand == null)
                return ErrorHandler.BadRequestResponse(this, "Brand details cannot be null");

            await _catalogService.AddBrand(brand);
            return Ok("Brand added successfully");
        }

        /// <summary>
        /// Updates an existing brand.
        /// </summary>
        /// <param name="brand">The updated brand details.</param>
        /// <param name="id">The ID of the brand to update.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Brand updated successfully.</response>
        /// <response code="404">Brand not found.</response>
        [HttpPut("brands/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateBrand(BrandDTO brand, int id)
        {
            var existingBrand = await _catalogService.GetBrandById(id);
            if (existingBrand == null)
                return ErrorHandler.NotFoundResponse(this, $"Brand with ID {id} not found");

            await _catalogService.UpdateBrand(brand, id);
            return Ok("Brand updated successfully");
        }

        /// <summary>
        /// Deletes a brand by ID.
        /// </summary>
        /// <param name="id">The ID of the brand to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Brand deleted successfully.</response>
        /// <response code="404">Brand not found.</response>
        [HttpDelete("brands/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            var existingBrand = await _catalogService.GetBrandById(id);
            if (existingBrand == null)
                return ErrorHandler.NotFoundResponse(this, $"Brand with ID {id} not found");

            await _catalogService.DeleteBrand(id);
            return Ok("Brand deleted successfully");
        }

        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        /// <response code="200">Returns the list of categories.</response>
        /// <response code="404">No categories found.</response>
        [HttpGet("categories")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<CategoryDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var response = await _catalogService.GetAllCategories();
            if (response == null || !response.Any())
                return ErrorHandler.NotFoundResponse(this, "No categories found");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The requested category.</returns>
        /// <response code="200">Returns the requested category.</response>
        /// <response code="404">Category not found.</response>
        [HttpGet("categories/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var response = await _catalogService.GetCategoryById(id);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"Category with ID {id} not found");

            return Ok(response);
        }

        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="category">The details of the category to add.</param>
        /// <returns>Status of the creation operation.</returns>
        /// <response code="200">Category added successfully.</response>
        /// <response code="400">Invalid request or null input.</response>
        [HttpPost("categories")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddCategory(CategoryDTO category)
        {
            if (category == null)
                return ErrorHandler.BadRequestResponse(this, "Category details cannot be null");

            await _catalogService.AddCategory(category);
            return Ok("Category added successfully");
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="category">The updated category details.</param>
        /// <param name="id">The ID of the category to update.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Category updated successfully.</response>
        /// <response code="404">Category not found.</response>
        [HttpPut("categories/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateCategory(CategoryDTO category, int id)
        {
            var existingCategory = await _catalogService.GetCategoryById(id);
            if (existingCategory == null)
                return ErrorHandler.NotFoundResponse(this, $"Category with ID {id} not found");

            await _catalogService.UpdateCategory(category, id);
            return Ok("Category updated successfully");
        }

        /// <summary>
        /// Deletes a category by ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Category deleted successfully.</response>
        /// <response code="404">Category not found.</response>
        [HttpDelete("categories/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _catalogService.GetCategoryById(id);
            if (existingCategory == null)
                return ErrorHandler.NotFoundResponse(this, $"Category with ID {id} not found");

            await _catalogService.DeleteCategory(id);
            return Ok("Category deleted successfully");
        }

        /// <summary>
        /// Retrieves all colors.
        /// </summary>
        /// <returns>A list of all colors.</returns>
        /// <response code="200">Returns the list of colors.</response>
        /// <response code="404">No colors found.</response>
        [HttpGet("colors")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<ColorDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ColorDTO>>> GetAllColors()
        {
            var response = await _catalogService.GetAllColors();
            if (response == null || !response.Any())
                return ErrorHandler.NotFoundResponse(this, "No colors found");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a color by ID.
        /// </summary>
        /// <param name="id">The ID of the color.</param>
        /// <returns>The requested color.</returns>
        /// <response code="200">Returns the requested color.</response>
        /// <response code="404">Color not found.</response>
        [HttpGet("colors/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ColorDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ColorDTO>> GetColorById(int id)
        {
            var response = await _catalogService.GetColorById(id);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"Color with ID {id} not found");

            return Ok(response);
        }

        /// <summary>
        /// Adds a new color.
        /// </summary>
        /// <param name="color">The details of the color to add.</param>
        /// <returns>Status of the creation operation.</returns>
        /// <response code="200">Color added successfully.</response>
        /// <response code="400">Invalid request or null input.</response>
        [HttpPost("colors")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddColor(ColorDTO color)
        {
            if (color == null)
                return ErrorHandler.BadRequestResponse(this, "Color details cannot be null");

            await _catalogService.AddColor(color);
            return Ok("Color added successfully");
        }

        /// <summary>
        /// Updates an existing color.
        /// </summary>
        /// <param name="color">The updated color details.</param>
        /// <param name="id">The ID of the color to update.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Color updated successfully.</response>
        /// <response code="404">Color not found.</response>
        [HttpPut("colors/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateColor(ColorDTO color, int id)
        {
            var existingColor = await _catalogService.GetColorById(id);
            if (existingColor == null)
                return ErrorHandler.NotFoundResponse(this, $"Color with ID {id} not found");

            await _catalogService.UpdateColor(color, id);
            return Ok("Color updated successfully");
        }

        /// <summary>
        /// Deletes a color by ID.
        /// </summary>
        /// <param name="id">The ID of the color to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Color deleted successfully.</response>
        /// <response code="404">Color not found.</response>
        [HttpDelete("colors/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteColor(int id)
        {
            var existingColor = await _catalogService.GetColorById(id);
            if (existingColor == null)
                return ErrorHandler.NotFoundResponse(this, $"Color with ID {id} not found");

            await _catalogService.DeleteColor(id);
            return Ok("Color deleted successfully");
        }

        /// <summary>
        /// Retrieves all sizes.
        /// </summary>
        /// <returns>A list of all sizes.</returns>
        /// <response code="200">Returns the list of sizes.</response>
        /// <response code="404">No sizes found.</response>
        [HttpGet("sizes")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<SizeDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<SizeDTO>>> GetAllSizes()
        {
            var response = await _catalogService.GetAllSizes();
            if (response == null || !response.Any())
                return ErrorHandler.NotFoundResponse(this, "No sizes found");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves a size by ID.
        /// </summary>
        /// <param name="id">The ID of the size.</param>
        /// <returns>The requested size.</returns>
        /// <response code="200">Returns the requested size.</response>
        /// <response code="404">Size not found.</response>
        [HttpGet("sizes/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(SizeDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SizeDTO>> GetSizeById(int id)
        {
            var response = await _catalogService.GetSizeById(id);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"Size with ID {id} not found");

            return Ok(response);
        }

        /// <summary>
        /// Adds a new size.
        /// </summary>
        /// <param name="size">The details of the size to add.</param>
        /// <returns>Status of the creation operation.</returns>
        /// <response code="200">Size added successfully.</response>
        /// <response code="400">Invalid request or null input.</response>
        [HttpPost("sizes")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddSize(SizeDTO size)
        {
            if (size == null)
                return ErrorHandler.BadRequestResponse(this, "Size details cannot be null");

            await _catalogService.AddSize(size);
            return Ok("Size added successfully");
        }

        /// <summary>
        /// Updates an existing size.
        /// </summary>
        /// <param name="size">The updated size details.</param>
        /// <param name="id">The ID of the size to update.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Size updated successfully.</response>
        /// <response code="404">Size not found.</response>
        [HttpPut("sizes/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateSize(SizeDTO size, int id)
        {
            var existingSize = await _catalogService.GetSizeById(id);
            if (existingSize == null)
                return ErrorHandler.NotFoundResponse(this, $"Size with ID {id} not found");

            await _catalogService.UpdateSize(size, id);
            return Ok("Size updated successfully");
        }

        /// <summary>
        /// Deletes a size by ID.
        /// </summary>
        /// <param name="id">The ID of the size to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Size deleted successfully.</response>
        /// <response code="404">Size not found.</response>
        [HttpDelete("sizes/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteSize(int id)
        {
            var existingSize = await _catalogService.GetSizeById(id);
            if (existingSize == null)
                return ErrorHandler.NotFoundResponse(this, $"Size with ID {id} not found");

            await _catalogService.DeleteSize(id);
            return Ok("Size deleted successfully");
        }
    }
}
