using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace dotnetCoreInterviewPrepDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowMyOrigin")]
    public class SecurityController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<SecurityController> _logger;
        private readonly string _jwtSecretKey;

        public SecurityController(IConfiguration config, ILogger<SecurityController> logger)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            _jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
                ?? _config["Jwt:SecretKey"] 
                ?? throw new InvalidOperationException("JWT secret key not configured");
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token");
                throw;
            }
        }

        private bool IsValidUser(User user)
        {
            // Add your user validation logic here
            // This is a simple example - in production, you should check against a database
            return !string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password);
        }

        private bool HasRequiredPermissions(string userName)
        {
            // Add your permission checking logic here
            // This is a simple example - in production, you should check against your authorization system
            return true;
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            try
            {
                if (string.IsNullOrEmpty(user.UserName))
                {
                    return BadRequest("Username is required");
                }
                // if (!IsValidUser(user))
                // {
                //     return Unauthorized("Invalid credentials");
                // }
                // if (!HasRequiredPermissions(user.Username))
                // {
                //     return Forbidden("Insufficient permissions");
                // }

                if (user.UserName == "test") // 
                {
                    var token = GenerateJwtToken(user);
                    
                    return Ok(new TokenResponse 
                    { 
                        Token = token,
                        ExpiresIn = 1800 // 30 minutes in seconds
                    });
                }

                return Unauthorized("Invalid credentials");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {UserName}", user.UserName);
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while processing your request.");
            }
        }
    }

    public class User
    {
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class TokenResponse
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
