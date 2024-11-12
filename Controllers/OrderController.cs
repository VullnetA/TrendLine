using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.DTOs;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var response = await _orderService.GetAllOrders();
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Advanced User, Customer")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateOrder(CreateOrderDTO orderDto)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Customer ID claim not found in token");
                }

                await _orderService.CreateOrder(orderDto, userId);
                return Ok();
            }
            else
            {
                return Unauthorized("User not authenticated");
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult> UpdateOrderStatus(string status, int id)
        {
            await _orderService.UpdateOrderStatus(status, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Advanced User, Customer")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return Ok();
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByStatus(string status)
        {
            var response = await _orderService.GetOrdersByStatus(status);
            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpGet("dateRange")]
        [Authorize(Roles = "Admin, Advanced User")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            // Ensure DateTime values are in UTC
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            var response = await _orderService.GetOrdersByDateRange(startDate, endDate);
            if (response == null) return NotFound();
            return Ok(response);
        }


        [HttpGet("{orderId}/items")]
        [Authorize(Roles = "Admin, Advanced User, Customer")]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetOrderItems(int orderId)
        {
            var response = await _orderService.GetOrderItemsByOrderId(orderId);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
