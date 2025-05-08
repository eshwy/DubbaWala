using Dabbawalla.Dto;
using Dabbawalla.Models;
using Dabbawalla.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dabbawalla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly EmailListenerService _emailListenerService;
        public LoginController(MyDbContext dbContext, IConfiguration configuration, UserService userService)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var userService = new UserService(_dbContext, _emailListenerService); // Inject this in a real-world app

            var user = userService.ValidateUser(loginDto.EmailAddress, loginDto.Password);

            if (user == null)
            {
                return BadRequest("Invalid Email or PassWord.");
            }

            // Create JWT token (as described in the previous implementation)
            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterUserDto registerUserDto)
        {
            var result = await _userService.RegisterUser(registerUserDto);
            if (result)
            {
                return Ok("User registered successfully.");
            }
            else
            {
                return BadRequest("Email already in exist");
            }
        }

        [HttpPost("registerVendor")]
        public async Task<IActionResult> RegisterVendor([FromBody] RegisterVendorDto registerVendorDto)
        {
            var result = await _userService.RegisterVendor(registerVendorDto);
            if (result)
            {
                return Ok("Vendor registered successfully.");
            }
            else
            {
                return BadRequest("Email already in exist ");
            }
        }

        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await _userService.ChangePassword(changePasswordDto.EmailAddress);
            if (result)
            {
                return Ok("Password has been changed successfully.");
            }
            else
            {
                return BadRequest("Invalid email ID.");
            }
        }


        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create claims based on user and role
            var claims = new[]
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Name", user.Name),
                new Claim("Email", user.EmailAddress ?? string.Empty),
                new Claim("Role", _dbContext.Roles.Where(x=>x.RoleId==user.RoleId).FirstOrDefault().RoleName)
            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(24), // Set token expiration to 24 hours
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize]
        [HttpGet("GetUserDetails")]
        public IActionResult GetUserDetails()
        {
            // Access the claims
            var userClaims = HttpContext.User;

            // Extract specific claims
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value; // User ID
            var email = userClaims.FindFirst(ClaimTypes.Email)?.Value; // Email
            var role = userClaims.FindFirst(ClaimTypes.Role)?.Value; // Role
                                                                     // Add any additional claims as necessary

            var userDetails = new
            {
                UserId = userId,
                Email = email,
                Role = role
                // Add any additional properties
            };

            return Ok(userDetails);
        }
    }
}
