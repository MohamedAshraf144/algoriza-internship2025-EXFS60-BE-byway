using AutoMapper;
using Byway.Application.DTOs.Auth;
using Byway.Application.Interfaces;
using Byway.Domain.Interfaces.IRepositories;
using Byway.Domain.Entities;
using Byway.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Byway.Application.Services
{
	public class AuthService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;
		private readonly IEmailService _emailService;

		public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IEmailService emailService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_configuration = configuration;
			_emailService = emailService;
		}

		public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
		{
			// Check if user already exists
			var existingUser = await _unitOfWork.Repository<User>()
				.FindAsync(u => u.Email == dto.Email);

			if (existingUser.Any())
			{
				throw new Exception("User with this email already exists");
			}

			// Create new user
			var user = _mapper.Map<User>(dto);
			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
			user.Role = UserRole.Student;
			user.CreatedAt = DateTime.UtcNow;

			await _unitOfWork.Repository<User>().AddAsync(user);
			await _unitOfWork.SaveChangesAsync();

			// Generate JWT token
			var token = await GenerateJwtTokenAsync(user);

			// Send welcome email
			await _emailService.SendWelcomeEmailAsync(user.Email, user.FirstName);

			return new AuthResponseDto
			{
				Token = token,
				UserId = user.Id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = user.Role.ToString()
			};
		}

		public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
		{
			// Find user by email
			var users = await _unitOfWork.Repository<User>()
				.FindAsync(u => u.Email == dto.Email);

			var user = users.FirstOrDefault();
			if (user == null)
			{
				throw new Exception("Invalid email or password");
			}

			// Verify password
			if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
			{
				throw new Exception("Invalid email or password");
			}

			// Generate JWT token
			var token = await GenerateJwtTokenAsync(user);

			return new AuthResponseDto
			{
				Token = token,
				UserId = user.Id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = user.Role.ToString()
			};
		}

		public async Task<bool> LogoutAsync(string userId)
		{
			// In a real application, you would invalidate the token
			// This could be done by maintaining a blacklist of tokens
			// For now, we'll just return true
			await Task.CompletedTask;
			return true;
		}

		public async Task<AuthResponseDto> CreateAdminAsync(RegisterDto dto)
		{
			// Check if user already exists
			var existingUser = await _unitOfWork.Repository<User>()
				.FindAsync(u => u.Email == dto.Email);

			if (existingUser.Any())
			{
				throw new Exception("User with this email already exists");
			}

			// Create new admin user
			var user = _mapper.Map<User>(dto);
			user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
			user.Role = UserRole.Admin; // Set as Admin
			user.CreatedAt = DateTime.UtcNow;

			await _unitOfWork.Repository<User>().AddAsync(user);
			await _unitOfWork.SaveChangesAsync();

			// Generate JWT token
			var token = await GenerateJwtTokenAsync(user);

			return new AuthResponseDto
			{
				Token = token,
				UserId = user.Id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = user.Role.ToString()
			};
		}

		public async Task<string> GenerateJwtTokenAsync(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
				new Claim(ClaimTypes.Role, user.Role.ToString())
			};

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
				Issuer = _configuration["Jwt:Issuer"],
				Audience = _configuration["Jwt:Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return await Task.FromResult(tokenHandler.WriteToken(token));
		}
	}
}
