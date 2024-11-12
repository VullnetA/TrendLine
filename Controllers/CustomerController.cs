using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrendLine.DTOs;
using TrendLine.Services.Interfaces;

namespace TrendLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
        {
            var response = await _customerService.GetAllCustomers();
            if (response == null) return NotFound();
            return Ok(response);
        }

        // Get customer details by ID (accessible by Admin and the customer themselves)
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Customer")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(string id)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // Admins can access any customer, but customers can only access their own data
            if (User.IsInRole("Customer") && currentUserId != id)
            {
                return Forbid();
            }

            var customer = await _customerService.GetCustomerById(id);
            if (customer == null) return NotFound("Customer not found");

            return Ok(customer);
        }

        // Delete a customer (accessible by Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteCustomer(string id)
        {
            await _customerService.DeleteCustomer(id);

            return Ok("Customer deleted successfully");
        }
    }
}
