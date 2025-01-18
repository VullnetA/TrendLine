using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.DTOs;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            var response = await _productService.GetAllProducts();
            return Ok(response ?? new List<ProductDTO>());
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var response = await _productService.GetProductById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult> AddProduct(AddProductDTO addProduct)
        {
            await _productService.AddProduct(addProduct);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProduct(id);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult> UpdateProduct(EditProductDTO editProduct, int id)
        {
            await _productService.UpdateProduct(editProduct, id);
            return Ok();
        }

        // The following are seaparate methods to filter products by different attributes
        [HttpGet("byCategory/{category}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByCategory(string category)
        {
            var response = await _productService.FindByCategory(category);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("byBrand/{brand}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByBrand(string brand)
        {
            var response = await _productService.FindByBrand(brand);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("byGender/{gender}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByGender(string gender)
        {
            var response = await _productService.FindByGender(gender);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("byPriceRange")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByPriceRange(double minPrice, double maxPrice)
        {
            var response = await _productService.FindByPriceRange(minPrice, maxPrice);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("bySize/{size}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsBySize(string size)
        {
            var response = await _productService.FindBySize(size);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("byColor/{color}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsByColor(string color)
        {
            var response = await _productService.FindByColor(color);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("countByCategory/{category}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        public async Task<ActionResult<long>> CountProductsByCategory(string category)
        {
            var response = await _productService.CountByCategory(category);
            return Ok(response);
        }

        [HttpGet("countByBrand/{brand}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        public async Task<ActionResult<long>> CountProductsByBrand(string brand)
        {
            var response = await _productService.CountByBrand(brand);
            return Ok(response);
        }

        [HttpGet("countAvailable")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<long>> CountAvailableProducts()
        {
            var response = await _productService.CountByAvailability();
            return Ok(response);
        }

        [HttpGet("countOutOfStock")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<long>> CountOutOfStockProducts()
        {
            var response = await _productService.CountOutOfStock();
            return Ok(response);
        }

        [HttpGet("quantity/{id}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        public async Task<ActionResult<ProductQuantityDTO>> GetQuantity(int id)
        {
            var response = await _productService.GetProductQuantity(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPut("quantity/{id}")]
        [Authorize(Roles = "Admin, Advanced User, Simple User")]
        public async Task<ActionResult> UpdateProductQuantity(UpdateQuantityDTO updateQuantity, int id)
        {
            if (updateQuantity.Quantity < 0)
            {
                return BadRequest("Quantity cannot be negative.");
            }

            await _productService.UpdateQuantity(id, updateQuantity.Quantity);
            return Ok("Product quantity updated successfully.");
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Advanced User, Simple User, Customer")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> SearchProducts([FromQuery] ProductSearchDTO searchParams)
        {
            var products = await _productService.SearchProducts(searchParams);

            // Return an empty array with a 200 OK status if no products match the search criteria
            if (!products.Any())
            {
                return Ok(new List<ProductDTO>());
            }

            return Ok(products);
        }
    }
}
