using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrendLine.DTOs;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Manages order-related operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")] // Version 1.0
    [ApiVersion("2.0")] // Version 2.0 for future updates
    [Route("api/v{version:apiVersion}/[controller]")] // Include version in the route
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">Service for order management.</param>
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Retrieves all orders.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        /// <response code="200">Returns the list of orders.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        [MapToApiVersion("1.0")] // Available in version 1.0
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(IEnumerable<OrderDTO>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var response = await _orderService.GetAllOrders();
            if (response == null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a specific order by ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>The requested order.</returns>
        /// <response code="200">Returns the requested order.</response>
        /// <response code="404">Order not found.</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")] // Available in version 1.0
        [Authorize(Roles = "Admin, Advanced User, Customer")]
        [ProducesResponseType(typeof(OrderDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderById(id);
            if (response == null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="orderDto">Details of the order to create.</param>
        /// <returns>Status of the creation operation.</returns>
        /// <response code="200">Order created successfully.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpPost]
        [MapToApiVersion("1.0")] // Available in version 1.0
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
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

        /// <summary>
        /// Updates the status of an order.
        /// </summary>
        /// <param name="status">The new status.</param>
        /// <param name="id">The ID of the order.</param>
        /// <returns>Status of the update operation.</returns>
        /// <response code="200">Order status updated successfully.</response>
        [HttpPut("{id}/status")]
        [MapToApiVersion("1.0")] // Available in version 1.0
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> UpdateOrderStatus(string status, int id)
        {
            await _orderService.UpdateOrderStatus(status, id);
            return Ok();
        }

        /// <summary>
        /// Deletes an order by ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Order deleted successfully.</response>
        /// <response code="404">Order not found.</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")] // Available in version 1.0
        [Authorize(Roles = "Admin, Advanced User, Customer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteOrder(id);
            return Ok();
        }

        /// <summary>
        /// Retrieves orders by status.
        /// </summary>
        /// <param name="status">The status to filter by.</param>
        /// <returns>A list of orders with the specified status.</returns>
        /// <response code="200">Returns the filtered list of orders.</response>
        /// <response code="404">No orders found with the specified status.</response>
        [HttpGet("status/{status}")]
        [MapToApiVersion("1.0")] // Available in version 1.0
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(IEnumerable<OrderDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByStatus(string status)
        {
            var response = await _orderService.GetOrdersByStatus(status);
            if (response == null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Retrieves orders within a specified date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>A list of orders within the date range.</returns>
        /// <response code="200">Returns the filtered list of orders.</response>
        /// <response code="404">No orders found within the date range.</response>
        [HttpGet("dateRange")]
        [MapToApiVersion("1.0")] // Available in version 1.0
        [Authorize(Roles = "Admin, Advanced User")]
        [ProducesResponseType(typeof(IEnumerable<OrderDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrdersByDateRange(DateTime startDate, DateTime endDate)
        {
            // Ensuring DateTime values are in UTC due to conversion issues
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            var response = await _orderService.GetOrdersByDateRange(startDate, endDate);
            if (response == null) return NotFound();
            return Ok(response);
        }

        /// <summary>
        /// Retrieves the items of a specific order by order ID.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <returns>A list of order items.</returns>
        /// <response code="200">Returns the list of order items.</response>
        /// <response code="404">Order not found.</response>
        [HttpGet("{orderId}/items")]
        [MapToApiVersion("2.0")] // New in version 2.0
        [Authorize(Roles = "Admin, Advanced User, Customer")]
        [ProducesResponseType(typeof(IEnumerable<OrderItemDTO>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetOrderItems(int orderId)
        {
            var response = await _orderService.GetOrderItemsByOrderId(orderId);
            if (response == null) return NotFound();
            return Ok(response);
        }
    }
}
