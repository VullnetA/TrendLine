using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrendLine.Data;
using TrendLine.DTOs;
using TrendLine.Models;
using TrendLine.Services.AuthenticationService;
using TrendLine.Services.Helpers;

namespace TrendLine.Controllers
{
    /// <summary>
    /// Handles authentication-related operations such as registration, login, and role management.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userManager">Service for managing users.</param>
        /// <param name="roleManager">Service for managing roles.</param>
        /// <param name="context">Application database context.</param>
        /// <param name="tokenService">Service for generating authentication tokens.</param>
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

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration request containing user details.</param>
        /// <returns>Status of the registration operation.</returns>
        /// <response code="200">User created successfully.</response>
        /// <response code="400">User already exists or invalid request.</response>
        [HttpPost("register")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Register(RegistrationRequestDTO request)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (userExists != null)
            {
                return ErrorHandler.BadRequestResponse(this, "User already exists");
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
                return ErrorHandler.BadRequestResponse(this, "User creation failed", result.Errors);
            }

            return Ok("User created successfully");
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <param name="request">The authentication request containing email and password.</param>
        /// <returns>An authentication response containing the JWT token and user details.</returns>
        /// <response code="200">Authentication successful.</response>
        /// <response code="400">Invalid credentials or request.</response>
        /// <response code="401">Unauthorized access.</response>
        [HttpPost("login")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AuthResponseDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<AuthResponseDTO>> Authenticate([FromBody] AuthRequestDTO request)
        {
            if (request == null)
            {
                return ErrorHandler.BadRequestResponse(this, "Request body is empty");
            }

            var managedUser = await _userManager.FindByEmailAsync(request.Email);
            if (managedUser == null)
            {
                return ErrorHandler.BadRequestResponse(this, "No user found");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
            if (!isPasswordValid)
            {
                return ErrorHandler.BadRequestResponse(this, "Bad credentials");
            }

            var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (userInDb is null)
            {
                return ErrorHandler.UnauthorizedResponse(this, "Authentication failed");
            }

            var roles = await _userManager.GetRolesAsync(managedUser);

            var accessToken = await _tokenService.CreateToken(userInDb);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponseDTO
            {
                UserId = userInDb.Id,
                Email = userInDb.Email,
                Token = accessToken,
                Roles = roles
            });
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        /// <returns>Status of the role creation operation.</returns>
        /// <response code="200">Role created successfully.</response>
        [HttpPost("role")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> CreateRoles(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return Ok();
        }

        /// <summary>
        /// Assigns a role to a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>Status of the role assignment operation.</returns>
        /// <response code="200">Role assigned successfully.</response>
        /// <response code="400">Invalid username or role name.</response>
        [HttpPost("assign")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AssignRoleToUser(string username, string roleName)
        {
            try
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
            catch (ApplicationException ex)
            {
                return ErrorHandler.BadRequestResponse(this, ex.Message);
            }
        }

        /// <summary>
        /// Registers a new customer.
        /// </summary>
        /// <param name="registrationDto">The registration details for the customer.</param>
        /// <returns>Status of the customer registration operation.</returns>
        /// <response code="200">Customer registered successfully.</response>
        /// <response code="400">Customer already exists or invalid request.</response>
        [HttpPost("registerCustomer")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> RegisterCustomer(CustomerRegistrationDTO registrationDto)
        {
            var userExists = await _userManager.FindByEmailAsync(registrationDto.Email);
            if (userExists != null)
            {
                return ErrorHandler.BadRequestResponse(this, "User already exists");
            }

            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole("Customer"));
                if (!roleResult.Succeeded)
                {
                    return ErrorHandler.InternalServerErrorResponse(this, "Error creating Customer role", new Exception("Role creation failed"));
                }
            }

            var user = new ApplicationUser
            {
                Email = registrationDto.Email,
                UserName = registrationDto.Email,
                FirstName = registrationDto.FirstName,
                LastName = registrationDto.LastName,
                Address = registrationDto.Address,
                PhoneNumber = registrationDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registrationDto.Password);
            if (!result.Succeeded)
            {
                return ErrorHandler.BadRequestResponse(this, "User creation failed", result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Customer");

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
