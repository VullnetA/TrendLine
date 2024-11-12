using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Discount>>> GetAllDiscounts()
        {
            var discounts = await _discountService.GetAllDiscounts();
            return Ok(discounts);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
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

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddDiscount(AddDiscountDTO discountDto)
        {
            await _discountService.AddDiscount(discountDto);
            return Ok("Discount added successfully");
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateDiscount(UpdateDiscountDTO discountDto)
        {
            await _discountService.UpdateDiscount(discountDto);
            return Ok("Discount updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteDiscount(int id)
        {
            await _discountService.DeleteDiscount(id);
            return Ok("Discount removed successfully");
        }
    }
}
