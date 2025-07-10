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
            return Ok(await _accountService.AuthenticateAsync(request));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            return Ok(await _accountService.RegisterBasicUserAsync(request, origin));
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("register-content-manager")]
        public async Task<IActionResult> RegisterContentManagerAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            return Ok(await _accountService.RegisterContentManagerUserAsync(request, origin));
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            return Ok(await _accountService.RegisterAdminUserAsync(request, origin));
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmAccountAsync([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _accountService.ConfirmAccountAsync(userId, token);
            return Ok(new { Message = result });
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers.Origin;
            return Ok(await _accountService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _accountService.ResetPasswordAsync(request));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var ipAddress = GetIpAddress();
            var response = await _accountService.RefreshTokenAsync(request.Token);
            if (response.HasError)
            {
                return BadRequest(new { message = response.Error });
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            var token = request.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            var ipAddress = GetIpAddress();
            var result = await _accountService.RevokeTokenAsync(token);
            if (!result)
            {
                return NotFound(new { message = "Token not found or already invalidated" });
            }
            return Ok(new { message = "Token revoked" });
        }

        private string GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
        }
    }
}
