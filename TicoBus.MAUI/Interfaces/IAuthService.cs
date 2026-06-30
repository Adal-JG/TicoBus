using TicoBus.MAUI.DTOs;

namespace TicoBus.MAUI.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResponse>?> LoginAsync(LoginRequest request);
    }
}