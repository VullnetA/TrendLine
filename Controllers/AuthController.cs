using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrendLine.Data;
using TrendLine.DTOs;
using TrendLine.Models;  // Ensure this namespace includes ApplicationUser
using TrendLine.Services.AuthenticationService;

namespace TrendLine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context,
            TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegistrationRequestDTO request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)
            {
                return BadRequest("User already exists");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User created successfully");
        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] AuthRequestDTO request)
        {
            if (request == null)
            {
                return BadRequest("Request body is empty");
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                return BadRequest("No user found");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (userInDb is null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(managedUser);

            var accessToken = await _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                UserId = userInDb.Id,
                Email = userInDb.Email,
                Token = accessToken,
                Roles = roles // Add this line to include the roles
            });
        }


        [HttpPost("role")]
        public async Task<ActionResult> CreateRoles(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return Ok();
        }

        [HttpPost("assign")]
        public async Task<ActionResult> AssignRoleToUser(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username)
                ?? throw new ApplicationException($"User with username '{username}' not found.");
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ApplicationException($"Role '{roleName}' does not exist.");
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                await _userManager.AddToRoleAsync(user, roleName);
            }
            return Ok();
        }

        [HttpPost("registerCustomer")]
        public async Task<ActionResult> RegisterCustomer(CustomerRegistrationDTO registrationDto)
        {
            // Check if the user already exists
            var userExists = await _userManager.FindByEmailAsync(registrationDto.Email);
            if (userExists != null) return BadRequest("User already exists");

            // Ensure the "Customer" role exists, and create it if it doesn't
            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Customer"));
                if (!roleResult.Succeeded)
                {
                    return StatusCode(500, "Error creating Customer role");
                }
            }

            // Create ApplicationUser and set required fields
            var user = new ApplicationUser
            {
                Email = registrationDto.Email,
                UserName = registrationDto.Email,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Address = registrationDto.Address,
                PhoneNumber = registrationDto.PhoneNumber
            };

            // Create the user in the database
            var result = await _userManager.CreateAsync(user, registrationDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            // Assign Customer role to the new user
            await _userManager.AddToRoleAsync(user, "Customer");

            // Create Customer entity with additional details
            var customer = new Customer
            {
                UserId = user.Id,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Address = registrationDto.Address,
                PhoneNumber = registrationDto.PhoneNumber
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok("Customer registered successfully");
        }


    }
}
