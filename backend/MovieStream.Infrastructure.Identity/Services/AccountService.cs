using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using MovieStream.Core.Application.DTOs.Account;
using MovieStream.Core.Application.Enums;
using MovieStream.Core.Application.Interfaces.Services;
using MovieStream.Core.Domain.Settings;
using MovieStream.Infrastructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using MovieStream.Infrastructure.Identity.Contexts;
using MovieStream.Core.Application.DTOs.Email;
using Microsoft.EntityFrameworkCore;

namespace MovieStream.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _jwtSettings;
        private readonly IdentityContext _identityContext;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IEmailService emailService, IOptions<JwtSettings> jwtSettings, IdentityContext identityContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtSettings = jwtSettings.Value;
            _identityContext = identityContext;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No account registered with {request.Email}.";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid email or password.";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            var refreshToken = GenerateRefreshToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.RefreshToken = refreshToken.Token;

            user.RefreshTokens.Add(refreshToken);
            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();

            return response;
        }

        public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshTokenValue)
        {
            var response = new AuthenticationResponse();
            var user = await _identityContext.Users.Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(u => u.Token == refreshTokenValue));

            if (user == null)
            {
                response.HasError = true;
                response.Error = "Invalid token: User not found.";
                return response;
            }

            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshTokenValue);

            if (refreshToken == null)
            {
                response.HasError = true;
                response.Error = "Invalid token: Refresh token not found for user.";
                return response;
            }

            if (!refreshToken.IsActive)
            {
                response.HasError = true;
                response.Error = "Invalid token: Refresh token is not active";
                return response;
            }

            var newRefreshToken = GenerateRefreshToken(user);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            user.RefreshTokens.Add(newRefreshToken);

            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();

            var jwtToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            response.RefreshToken = newRefreshToken.Token;

            return response;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _identityContext.Users.Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null) return false;

            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);

            if (refreshToken == null || !refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();

            return true;
        }

        public async Task<RegisterResponse> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new() { HasError = false };
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new AppUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
                var verificationUri = await SendVerificationEmailUri(user, origin);
                await _emailService.SendAsync(new EmailRequest()
                {
                    To = user.Email,
                    Subject = "Confirm Your MovieStream Account",
                    HtmlBodyTemplateName = "ConfirmAccount",
                    TemplateData = new Dictionary<string, object>
                    {
                        { "UserName", user.UserName },
                        { "VerificationUrl", verificationUri }
                    }
                });
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred while trying to register the user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }
            return response;
        }

        public async Task<RegisterResponse> RegisterContentManagerUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new() { HasError = false };
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new AppUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, nameof(Roles.ContentManager));
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred while trying to register the user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }
            return response;
        }

        public async Task<RegisterResponse> RegisterAdminUserAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new() { HasError = false };
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username '{request.UserName}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }

            var user = new AppUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, nameof(Roles.Admin));
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred while trying to register the user: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }
            return response;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "Error: User not found.";

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return $"Error confirming email: {string.Join(", ", result.Errors.Select(e => e.Description))}";

            return $"Account confirmed for {user.Email}. You can now use the app.";
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new() { HasError = false };
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No account registered with {request.Email}.";
                return response;
            }

            var resetTokenUri = await SendForgotPasswordUri(user, origin);
            await _emailService.SendAsync(new EmailRequest()
            {
                To = user.Email,
                Subject = "Reset Your MovieStream Password",
                HtmlBodyTemplateName = "ResetPassword",
                TemplateData = new Dictionary<string, object>
                {
                    { "UserName", user.UserName },
                    { "ResetUrl", resetTokenUri },
                    { "ExpirationHours", "2" } // TODO: Hacer esto configurable.
                }
            });

            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new() { HasError = false };
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"Error: User not found.";
                return response;
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Error: resetting password: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                return response;
            }
            return response;
        }

        private async Task<JwtSecurityToken> GenerateJWToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                new Claim("uid", user.Id)
            };
            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredential
            );
        }

        private async Task<string> SendVerificationEmailUri(AppUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/Account/confirm-email";
            var uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);
            return verificationUri;
        }

        private async Task<string> SendForgotPasswordUri(AppUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "reset-password";
            var uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(uri.ToString(), "token", code);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "email", user.Email);
            return verificationUri;
        }

        private Entities.RefreshToken GenerateRefreshToken(AppUser user)
        {
            var refreshToken = new Entities.RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                AppUserId = user.Id
            };
            return refreshToken;
        }

        private static string RandomTokenString(int length = 40)
        {
            using var rngCryptoServiceProvider = RandomNumberGenerator.Create();
            var randomBytes = new byte[length];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}