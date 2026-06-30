using TicoBus.MAUI.DTOs;
using TicoBus.MAUI.Interfaces;

namespace TicoBus.MAUI.Services
{
    public class AuthService : ApiClientBase, IAuthService
    {
        public async Task<ApiResponse<LoginResponse>?> LoginAsync(LoginRequest request)
        {
            return await PostApiResponseAsync<LoginResponse>("Auth/login", request);
        }
    }
}