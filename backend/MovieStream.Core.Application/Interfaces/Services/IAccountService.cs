using MovieStream.Core.Application.DTOs.Account;
using MovieStream.Core.Application.Wrappers;

namespace MovieStream.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<Response<RegisterResponse>> RegisterBasicUserAsync(RegisterRequest request, string origin);
        Task<Response<RegisterResponse>> RegisterAdminUserAsync(RegisterRequest request, string origin);
        Task<Response<RegisterResponse>> RegisterContentManagerUserAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmAccountAsync(string userId, string token);
        Task<Response<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<Response<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest request);
        Task<Response<AuthenticationResponse>> RefreshTokenAsync(string refreshToken);
        Task<Response<string>> RevokeTokenAsync(string token);
    }
}
