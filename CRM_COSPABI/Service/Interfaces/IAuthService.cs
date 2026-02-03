using CRM_COSPABI.DTOs;

namespace CRM_COSPABI.Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAdminAsync(LoginDto loginDto);
        Task<LoginResponseDto?> LoginClienteAsync(LoginDto loginDto);
    }
}
