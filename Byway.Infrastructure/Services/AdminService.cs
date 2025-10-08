using Microsoft.EntityFrameworkCore;
using Byway.Infrastructure.Data;
using Byway.Domain.Entities;
using Byway.Domain.Enums;
using Byway.Application.DTOs.Auth;
using Byway.Application.Interfaces;
using BCrypt.Net;

namespace Byway.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAdminAsync(CreateAdminDto dto)
        {
            try
            {
                // Check if admin already exists
                var existingAdmin = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == dto.Email);

                if (existingAdmin != null)
                {
                    return false;
                }

                // Create admin user
                var adminUser = new User
                {
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
                };

                _context.Users.Add(adminUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AdminExistsAsync()
        {
            return await _context.Users
                .AnyAsync(u => u.Role == UserRole.Admin);
        }

        public async Task<bool> DeleteAdminAsync()
        {
            try
            {
                var adminUsers = await _context.Users
                    .Where(u => u.Role == UserRole.Admin)
                    .ToListAsync();

                if (!adminUsers.Any())
                {
                    return false;
                }

                _context.Users.RemoveRange(adminUsers);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
