using MovieStream.Core.Application.DTOs.Account;
using MovieStream.Core.Application.Wrappers;

namespace MovieStream.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<Response> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task<Response> RegisterAdminUserAsync(RegisterRequest request, string origin);
        Task<Response> RegisterContentManagerUserAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmAccountAsync(string userId, string token);
        Task<Response> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<Response> ResetPasswordAsync(ResetPasswordRequest request);
        Task<Response<AuthenticationResponse>> RefreshTokenAsync(string refreshToken);
        Task<Response> RevokeTokenAsync(string token);
    }
}
