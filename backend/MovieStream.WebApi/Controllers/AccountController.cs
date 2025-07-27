using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStream.Core.Application.DTOs.Account;
using MovieStream.Core.Application.Enums;
using MovieStream.Core.Application.Interfaces.Services;

namespace MovieStream.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            var response = await _accountService.AuthenticateAsync(request);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            var response = await _accountService.RegisterBasicUserAsync(request, origin);
            return Ok(response);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("register-content-manager")]
        public async Task<IActionResult> RegisterContentManagerAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            var response = await _accountService.RegisterContentManagerUserAsync(request, origin);
            return Ok(response);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            var response = await _accountService.RegisterAdminUserAsync(request, origin);
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmAccountAsync([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _accountService.ConfirmAccountAsync(userId, token);
            return Ok(result);
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers.Origin;
            var response = await _accountService.ForgotPasswordAsync(request, origin);
            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _accountService.ResetPasswordAsync(request);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _accountService.RefreshTokenAsync(refreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest? request = null)
        {
            var token = request?.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            var result = await _accountService.RevokeTokenAsync(token);
            return Ok(result);
        }
    }
}
