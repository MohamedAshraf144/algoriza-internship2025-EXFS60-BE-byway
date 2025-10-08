using Byway.Application.DTOs.Auth;

namespace Byway.Application.Interfaces
{
    public interface IAdminService
    {
        Task<bool> CreateAdminAsync(CreateAdminDto dto);
        Task<bool> AdminExistsAsync();
        Task<bool> DeleteAdminAsync();
    }
}
