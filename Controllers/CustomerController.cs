using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrendLine.DTOs;
using TrendLine.Services.Helpers;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Manages customer-related operations.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="customerService">Service for managing customers.</param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>A list of all customers.</returns>
        /// <response code="200">Returns the list of customers.</response>
        /// <response code="404">No customers found.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<CustomerDTO>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomers();
            if (response == null || !response.Any())
                return ErrorHandler.NotFoundResponse(this, "No customers found");

            return Ok(response);
        }

        /// <summary>
        /// Retrieves customer details by ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>The requested customer details.</returns>
        /// <response code="200">Returns the requested customer details.</response>
        /// <response code="403">Forbidden. Customer attempting to access another customer's details.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin,Customer")]
        [ProducesResponseType(typeof(CustomerDTO), 200)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(string id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("Customer") && currentUserId != id)
            {
                return ErrorHandler.ForbiddenResponse(this, "Access to this resource is forbidden");
            }

            var customer = await _customerService.GetCustomerById(id);
            if (customer == null)
                return ErrorHandler.NotFoundResponse(this, "Customer not found");

            return Ok(customer);
        }

        /// <summary>
        /// Deletes a customer by ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>Status of the deletion operation.</returns>
        /// <response code="200">Customer deleted successfully.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            var customerExists = await _customerService.GetCustomerById(id);
            if (customerExists == null)
                return ErrorHandler.NotFoundResponse(this, "Customer not found");

            await _customerService.DeleteCustomer(id);
            return Ok("Customer deleted successfully");
        }
    }
}
