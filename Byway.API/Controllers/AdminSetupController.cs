using Microsoft.AspNetCore.Mvc;
using Byway.Application.Interfaces;
using Byway.Application.DTOs.Auth;

namespace Byway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminSetupController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminSetupController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto request)
        {
            try
            {
                var result = await _adminService.CreateAdminAsync(request);

                if (!result)
                {
                    return BadRequest(new { message = "Admin user already exists or error occurred" });
                }

                return Ok(new { 
                    message = "Admin user created successfully",
                    email = request.Email,
                    role = "Admin"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating admin user", error = ex.Message });
            }
        }

        [HttpGet("check-admin")]
        public async Task<IActionResult> CheckAdmin()
        {
            var adminExists = await _adminService.AdminExistsAsync();
            return Ok(new { adminExists });
        }

        [HttpDelete("delete-admin")]
        public async Task<IActionResult> DeleteAdmin()
        {
            try
            {
                var result = await _adminService.DeleteAdminAsync();
                
                if (!result)
                {
                    return BadRequest(new { message = "No admin user found to delete" });
                }

                return Ok(new { message = "Admin user deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting admin user", error = ex.Message });
            }
        }
    }
}
