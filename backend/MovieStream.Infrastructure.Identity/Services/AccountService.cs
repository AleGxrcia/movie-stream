using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieStream.Core.Application.DTOs.Account;
using MovieStream.Core.Application.DTOs.Email;
using MovieStream.Core.Application.Enums;
using MovieStream.Core.Application.Exceptions;
using MovieStream.Core.Application.Interfaces.Services;
using MovieStream.Core.Application.Wrappers;
using MovieStream.Core.Domain.Settings;
using MovieStream.Infrastructure.Identity.Contexts;
using MovieStream.Infrastructure.Identity.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No account registered with {request.Email}.", (int)HttpStatusCode.Unauthorized);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid email or password.", (int)HttpStatusCode.Unauthorized);
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            var refreshToken = GenerateRefreshToken(user);

            var response = new AuthenticationResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).ToList(),
                IsVerified = user.EmailConfirmed,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = refreshToken.Token,
            };

            user.RefreshTokens.Add(refreshToken);
            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();

            return new Response<AuthenticationResponse>(response, "User authenticated successfully.");
        }

        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(string refreshTokenValue)
        {
            var user = await _identityContext.Users.Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(u => u.Token == refreshTokenValue));

            if (user == null)
            {
                throw new ApiException("Invalid token: User not found.", (int)HttpStatusCode.BadRequest);
            }

            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshTokenValue);

            if (refreshToken == null)
            {
                throw new ApiException("Invalid token: Refresh token not found for user.", (int)HttpStatusCode.BadRequest);
            }

            if (!refreshToken.IsActive)
            {
                throw new ApiException("Invalid token: Refresh token is not active.", (int)HttpStatusCode.BadRequest);
            }

            var newRefreshToken = GenerateRefreshToken(user);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            user.RefreshTokens.Add(newRefreshToken);

            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();

            var jwtToken = await GenerateJWToken(user);

            var response = new AuthenticationResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = (await _userManager.GetRolesAsync(user).ConfigureAwait(false)).ToList(),
                IsVerified = user.EmailConfirmed,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = refreshToken.Token,
            };

            return new Response<AuthenticationResponse>(response, "Token refreshed successfully.");
        }

        public async Task<Response<string>> RevokeTokenAsync(string token)
        {
            var user = await _identityContext.Users.Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                throw new ApiException("Invalid token: User not found.", (int)HttpStatusCode.BadRequest);
            }

            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                throw new ApiException("Invalid token: Refresh token not found for user or is not active.",
                    (int)HttpStatusCode.BadRequest);
            }

            refreshToken.Revoked = DateTime.UtcNow;
            _identityContext.Update(user);
            await _identityContext.SaveChangesAsync();

            return new Response<string>(null, "Token revoked successfully.");
        }

        public async Task<Response<RegisterResponse>> RegisterBasicUserAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.", (int)HttpStatusCode.BadRequest);
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email '{request.Email}' is already registered.", (int)HttpStatusCode.BadRequest);
            }

            var user = new AppUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException($"An error occurred while trying to register the user:" +
                    $" {string.Join(", ", result.Errors.Select(e => e.Description))}", (int)HttpStatusCode.BadRequest);
            }

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

            var response = new RegisterResponse
            {
                HasError = false,
            };

            return new Response<RegisterResponse>(response,
                "User registered successfully. Please check your email to confirm your account."
            );
        }

        public async Task<Response<RegisterResponse>> RegisterContentManagerUserAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.", (int)HttpStatusCode.BadRequest);
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email '{request.Email}' is already registered.", (int)HttpStatusCode.BadRequest);
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
            if (!result.Succeeded)
            {
                throw new ApiException($"An error occurred while trying to register the user:" +
                    $" {string.Join(", ", result.Errors.Select(e => e.Description))}", (int)HttpStatusCode.BadRequest);
            }

            await _userManager.AddToRoleAsync(user, nameof(Roles.ContentManager));

            return new Response<RegisterResponse>(new RegisterResponse { HasError = false },
                "Content manager registered successfully."
            );
        }

        public async Task<Response<RegisterResponse>> RegisterAdminUserAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.", (int)HttpStatusCode.BadRequest);
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                throw new ApiException($"Email '{request.Email}' is already registered.", (int)HttpStatusCode.BadRequest);
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
            if (!result.Succeeded)
            {
                throw new ApiException($"An error occurred while trying to register the user:" +
                    $" {string.Join(", ", result.Errors.Select(e => e.Description))}", (int)HttpStatusCode.BadRequest);
            }

            await _userManager.AddToRoleAsync(user, nameof(Roles.Admin));

            return new Response<RegisterResponse>(new RegisterResponse { HasError = false },
                "Admin registered successfully."
            );
        }

        public async Task<Response<string>> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
            {
                throw new ApiException("User not found.", (int)HttpStatusCode.NotFound);
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new ApiException($"Error confirming email:" +
                    $" {string.Join(", ", result.Errors.Select(e => e.Description))}", (int)HttpStatusCode.BadRequest);
            }

            return new Response<string>(user.Id, $"Account confirmed for {user.Email}. You can now use the app.");
        }

        public async Task<Response<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No account registered with {request.Email}.", (int)HttpStatusCode.NotFound);
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

            return new Response<ForgotPasswordResponse>(new ForgotPasswordResponse { HasError = false },
                "Password reset link sent to your email."
            );
        }

        public async Task<Response<ResetPasswordResponse>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException("User not found.", (int)HttpStatusCode.NotFound);
            }

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password);
            if (!result.Succeeded)
            {
                throw new ApiException($"Error resetting password:" +
                    $" {string.Join(", ", result.Errors.Select(e => e.Description))}", (int)HttpStatusCode.BadRequest);
            }

            return new Response<ResetPasswordResponse>(new ResetPasswordResponse { HasError = false },
                "Password reset successfully."
            );
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