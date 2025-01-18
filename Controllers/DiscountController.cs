using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Interfaces;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Manages discount-related operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscountController"/> class.
        /// </summary>
        /// <param name="discountService">Service for managing discounts.</param>
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        /// <summary>
        /// Retrieves all discounts.
        /// </summary>
        /// <returns>A list of all discounts.</returns>
        /// <response code="200">Returns the list of discounts.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<Discount>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts()
        {
            var discounts = await _discountService.GetAllDiscounts();
            return Ok(discounts);
        }

        /// <summary>
        /// Retrieves a specific discount by ID.
        /// </summary>
        /// <param name="id">The ID of the discount.</param>
        /// <returns>The requested discount.</returns>
        /// <response code="200">Returns the requested discount.</response>
        /// <response code="404">Discount not found.</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Discount), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Discount>> GetDiscountById(int id)
        {
            try
            {
                var discount = await _discountService.GetDiscountById(id);
                return Ok(discount);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Discount not found");
            }
        }

        /// <summary>
        /// Adds a new discount.
        /// </summary>
        /// <param name="discountDto">Details of the discount to add.</param>
        /// <returns>Status of the creation operation.</returns>
        /// <response code="200">Discount added successfully.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> AddDiscount(AddDiscountDTO discountDto)
        {
            await _discountService.AddDiscount(discountDto);
            return Ok("Discount added successfully");
        }

        /// <summary>
        /// Updates an existing discount.
        /// </summary>
        /// <param name="discountDto">Details of the discount to update.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Discount updated successfully.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpPut]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> UpdateDiscount(UpdateDiscountDTO discountDto)
        {
            await _discountService.UpdateDiscount(discountDto);
            return Ok("Discount updated successfully");
        }

        /// <summary>
        /// Deletes a discount by ID.
        /// </summary>
        /// <param name="id">The ID of the discount.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Discount removed successfully.</response>
        /// <response code="404">Discount not found.</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("2.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteDiscount(int id)
        {
            var discount = await _discountService.GetDiscountById(id);
            if (discount == null) return NotFound("Discount not found");

            await _discountService.DeleteDiscount(id);
            return Ok("Discount removed successfully");
        }
    }
}
