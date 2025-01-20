using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.DTOs;
using TrendLine.Services.Helpers;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Manages product-related operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly LinkHelper _linkHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">Service for product management.</param>
        /// <param name="linkHelper">Utility for generating HATEOAS links to enhance API responses with navigable actions.</param>
        public ProductController(IProductService productService, LinkHelper linkHelper)
        {
            _productService = productService;
            _linkHelper = linkHelper;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A list of all products with HATEOAS links.</returns>
        /// <response code="200">Returns the list of products.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();

            if (products == null || !products.Any())
                return ErrorHandler.NotFoundResponse(this, "No products found");

            var userRoles = HttpContext.User.FindAll(System.Security.Claims.ClaimTypes.Role)
                                .Select(role => role.Value)
                                .ToList();

            foreach (var product in products)
            {
                product.Links = _linkHelper.GenerateProductLinksForAllProducts(
                    HttpContext,
                    product.Id,
                    product.Category,
                    userRoles
                );
            }

            return Ok(products);
        }

        /// <summary>
        /// Retrieves a specific product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The requested product.</returns>
        /// <response code="200">Returns the requested product.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(ProductDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
                return ErrorHandler.NotFoundResponse(this, $"Product with ID {id} not found");

            var userRoles = HttpContext.User.FindAll(System.Security.Claims.ClaimTypes.Role)
                                 .Select(role => role.Value)
                                 .ToList();

            product.Links = _linkHelper.GenerateProductLinksForSingleProduct(
                HttpContext,
                product.Id,
                product.Category,
                product.Brand,
                product.Gender,
                product.Size,
                product.Color,
                userRoles
            );

            return Ok(product);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="addProduct">Details of the product to add.</param>
        /// <returns>Status of the creation operation.</returns>
        /// <response code="200">Product added successfully.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> AddProduct(AddProductDTO addProduct)
        {
            await _productService.AddProduct(addProduct);
            return Ok("Product added successfully");
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Product removed successfully.</response>
        /// <response code="404">Product not found.</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return ErrorHandler.NotFoundResponse(this, $"Product with ID {id} not found");

            await _productService.DeleteProduct(id);
            return Ok("Product removed successfully");
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="editProduct">Details of the product to update.</param>
        /// <param name="id">The ID of the product.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Product updated successfully.</response>
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> UpdateProduct(EditProductDTO editProduct, int id)
        {
            await _productService.UpdateProduct(editProduct, id);
            return Ok("Product updated successfully");
        }

        /// <summary>
        /// Retrieves products by category.
        /// </summary>
        /// <param name="category">The category to filter by.</param>
        /// <returns>A list of products in the specified category.</returns>
        /// <response code="200">Returns the filtered list of products.</response>
        /// <response code="404">No products found in the specified category.</response>
        [HttpGet("byCategory/{category}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(string category)
        {
            var response = await _productService.FindByCategory(category);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"No products found in category '{category}'");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves products by brand.
        /// </summary>
        /// <param name="brand">The brand to filter by.</param>
        /// <returns>A list of products from the specified brand.</returns>
        /// <response code="200">Returns the filtered list of products.</response>
        /// <response code="404">No products found for the specified brand.</response>
        [HttpGet("byBrand/{brand}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByBrand(string brand)
        {
            var response = await _productService.FindByBrand(brand);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"No products found for brand '{brand}'");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves products by gender.
        /// </summary>
        /// <param name="gender">The gender to filter by.</param>
        /// <returns>A list of products for the specified gender.</returns>
        /// <response code="200">Returns the filtered list of products.</response>
        /// <response code="404">No products found for the specified gender.</response>
        [HttpGet("byGender/{gender}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByGender(string gender)
        {
            var response = await _productService.FindByGender(gender);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"No products found for gender '{gender}'");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves products within a price range.
        /// </summary>
        /// <param name="minPrice">The minimum price.</param>
        /// <param name="maxPrice">The maximum price.</param>
        /// <returns>A list of products within the price range.</returns>
        /// <response code="200">Returns the filtered list of products.</response>
        /// <response code="404">No products found within the price range.</response>
        [HttpGet("byPriceRange")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByPriceRange(double minPrice, double maxPrice)
        {
            var response = await _productService.FindByPriceRange(minPrice, maxPrice);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"No products found in the price range {minPrice} - {maxPrice}");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves products by size.
        /// </summary>
        /// <param name="size">The size to filter by.</param>
        /// <returns>A list of products in the specified size.</returns>
        /// <response code="200">Returns the filtered list of products.</response>
        /// <response code="404">No products found for the specified size.</response>
        [HttpGet("bySize/{size}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsBySize(string size)
        {
            var response = await _productService.FindBySize(size);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"No products found for size '{size}'");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves products by color.
        /// </summary>
        /// <param name="color">The color to filter by.</param>
        /// <returns>A list of products in the specified color.</returns>
        /// <response code="200">Returns the filtered list of products.</response>
        /// <response code="404">No products found for the specified color.</response>
        [HttpGet("byColor/{color}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByColor(string color)
        {
            var response = await _productService.FindByColor(color);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"No products found for color '{color}'");

            return Ok(response);
        }

        /// <summary>
        /// Counts products by category.
        /// </summary>
        /// <param name="category">The category to count products in.</param>
        /// <returns>The count of products in the specified category.</returns>
        /// <response code="200">Returns the count of products.</response>
        [HttpGet("countByCategory/{category}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<ActionResult<long>> CountProductsByCategory(string category)
        {
            var response = await _productService.CountByCategory(category);
            return Ok(response);
        }

        /// <summary>
        /// Counts products by brand.
        /// </summary>
        /// <param name="brand">The brand to count products in.</param>
        /// <returns>The count of products in the specified brand.</returns>
        /// <response code="200">Returns the count of products.</response>
        [HttpGet("countByBrand/{brand}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<ActionResult<long>> CountProductsByBrand(string brand)
        {
            var response = await _productService.CountByBrand(brand);
            return Ok(response);
        }

        /// <summary>
        /// Counts all available products.
        /// </summary>
        /// <returns>The count of available products.</returns>
        /// <response code="200">Returns the count of available products.</response>
        [HttpGet("countAvailable")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<ActionResult<long>> CountAvailableProducts()
        {
            var response = await _productService.CountByAvailability();
            return Ok(response);
        }

        /// <summary>
        /// Counts all out-of-stock products.
        /// </summary>
        /// <returns>The count of out-of-stock products.</returns>
        /// <response code="200">Returns the count of out-of-stock products.</response>
        [HttpGet("countOutOfStock")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(long), 200)]
        public async Task<ActionResult<long>> CountOutOfStockProducts()
        {
            var response = await _productService.CountOutOfStock();
            return Ok(response);
        }

        /// <summary>
        /// Retrieves the quantity of a specific product.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The quantity of the product.</returns>
        /// <response code="200">Returns the product quantity.</response>
        /// <response code="404">Product not found.</response>
        [HttpGet("quantity/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        [ProducesResponseType(typeof(ProductQuantityDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductQuantityDTO>> GetQuantity(int id)
        {
            var response = await _productService.GetProductQuantity(id);
            if (response == null)
                return ErrorHandler.NotFoundResponse(this, $"Product with ID {id} not found");

            return Ok(response);
        }

        /// <summary>
        /// Updates the quantity of a specific product.
        /// </summary>
        /// <param name="updateQuantity">The updated quantity details.</param>
        /// <param name="id">The ID of the product.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Product quantity updated successfully.</response>
        /// <response code="400">Invalid quantity value.</response>
        [HttpPut("quantity/{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateProductQuantity(UpdateQuantityDTO updateQuantity, int id)
        {
            if (updateQuantity.Quantity < 0)
            {
                return ErrorHandler.BadRequestResponse(this, "Quantity cannot be negative.");
            }

            await _productService.UpdateQuantity(id, updateQuantity.Quantity);
            return Ok("Product quantity updated successfully.");
        }

        /// <summary>
        /// Searches for products based on search parameters.
        /// </summary>
        /// <param name="searchParams">The search parameters.</param>
        /// <returns>A list of matching products.</returns>
        /// <response code="200">Returns the list of matching products.</response>
        /// <response code="404">No products matched the search parameters.</response>
        [HttpGet("search")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts([FromQuery] ProductSearchDTO searchParams)
        {
            var products = await _productService.SearchProducts(searchParams);
            if (products == null || !products.Any())
            {
                return ErrorHandler.NotFoundResponse(this, "No products matched the search parameters.");
            }

            return Ok(products);
        }
    }
}
