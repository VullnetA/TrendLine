using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrendLine.Data;
using TrendLine.Models;
using TrendLine.Repositories.Interfaces;
using TrendLine.Services.Implementations;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Manages Solr-based product search and indexing operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    [Authorize(Roles = "Admin")]
    public class SolrProductController : ControllerBase
    {
        private readonly SolrProductService _solrProductService;
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SolrProductController"/> class.
        /// </summary>
        /// <param name="solrProductService">Service for Solr operations.</param>
        /// <param name="context">Database context.</param>
        public SolrProductController(SolrProductService solrProductService, AppDbContext context)
        {
            _solrProductService = solrProductService;
            _context = context;
        }

        /// <summary>
        /// Synchronizes all products from the database to Solr index.
        /// </summary>
        /// <returns>A message indicating the number of products synchronized.</returns>
        /// <response code="200">Products successfully synchronized.</response>
        /// <response code="500">Internal server error occurred during synchronization.</response>
        [HttpPost("sync")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SyncProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .Include(p => p.Color)
                    .Include(p => p.Size)
                    .Include(p => p.Discount)
                    .ToListAsync();

                foreach (var product in products)
                {
                    await _solrProductService.AddOrUpdateProduct(product);
                }

                return Ok(new { message = $"Synchronized {products.Count} products" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Indexes a single product in Solr.
        /// </summary>
        /// <param name="product">The product to index.</param>
        /// <returns>Status of the indexing operation.</returns>
        /// <response code="200">Product successfully indexed.</response>
        /// <response code="400">Invalid product data.</response>
        [HttpPost("index")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IndexProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { error = "Product data cannot be null" });

            await _solrProductService.AddOrUpdateProduct(product);
            return Ok(new { message = "Product indexed successfully" });
        }

        /// <summary>
        /// Deletes a product from the Solr index.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Product successfully deleted from index.</response>
        /// <response code="404">Product not found in index.</response>
        [HttpDelete("delete/{id}")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _solrProductService.DeleteProduct(id);
                return Ok(new { message = "Product deleted from Solr index" });
            }
            catch (Exception)
            {
                return NotFound(new { error = $"Product with ID {id} not found in index" });
            }
        }

        /// <summary>
        /// Searches for products using various criteria.
        /// </summary>
        /// <param name="query">Search query string.</param>
        /// <param name="categoryId">Optional category ID filter.</param>
        /// <param name="brandId">Optional brand ID filter.</param>
        /// <param name="minPrice">Optional minimum price filter.</param>
        /// <param name="maxPrice">Optional maximum price filter.</param>
        /// <param name="sortBy">Sort criteria (relevance, price_asc, price_desc).</param>
        /// <param name="fuzzy">Enable fuzzy search matching.</param>
        /// <returns>List of matching products.</returns>
        /// <response code="200">Returns the list of matching products.</response>
        /// <response code="400">Invalid search parameters.</response>
        [HttpGet("search")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string query,
            [FromQuery] int? categoryId,
            [FromQuery] int? brandId,
            [FromQuery] double? minPrice,
            [FromQuery] double? maxPrice,
            [FromQuery] string sortBy = "relevance",
            [FromQuery] bool fuzzy = false)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest(new { error = "Search query cannot be empty" });

            var results = await _solrProductService.SearchProducts(
                query, categoryId, brandId, minPrice, maxPrice, sortBy, fuzzy);
            return Ok(results);
        }
    }
}