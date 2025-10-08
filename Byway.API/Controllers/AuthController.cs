using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Byway.Application.Services;
using Byway.Application.DTOs.Auth;
using Byway.Application.Interfaces;
using Byway.Domain.Entities;
using Byway.Domain.Enums;
using Byway.Domain.Interfaces.IRepositories;

namespace Byway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
                
                if (!result.Succeeded)
                {
                    var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
                    return Redirect($"{frontendUrl}/login?error=google_auth_failed");
                }

                var claims = result.Principal.Claims;
                var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                var firstName = claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var lastName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;

                if (string.IsNullOrEmpty(email))
                {
                    var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
                    return Redirect($"{frontendUrl}/login?error=email_not_found");
                }

                // Check if user exists
                var existingUsers = await _unitOfWork.Repository<User>().FindAsync(u => u.Email == email);
                var user = existingUsers.FirstOrDefault();

                if (user == null)
                {
                    // Create new user
                    user = new User
                    {
                        Email = email,
                        FirstName = firstName ?? "Google",
                        LastName = lastName ?? "User",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Random password for OAuth users
                        Role = UserRole.Student,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _unitOfWork.Repository<User>().AddAsync(user);
                    await _unitOfWork.SaveChangesAsync();

                    // Send welcome email
                    var emailService = HttpContext.RequestServices.GetRequiredService<IEmailService>();
                    await emailService.SendWelcomeEmailAsync(user.Email, user.FirstName);
                }

                // Generate JWT token
                var token = await GenerateJwtTokenAsync(user);

                // Redirect to frontend with token
                var redirectUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
                return Redirect($"{redirectUrl}/auth/google-success?token={token}&userId={user.Id}&email={user.Email}&firstName={user.FirstName}&lastName={user.LastName}&role={user.Role}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Google OAuth Error: {ex.Message}");
                var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";
                return Redirect($"{frontendUrl}/login?error=google_auth_error");
            }
        }

        private async Task<string> GenerateJwtTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}