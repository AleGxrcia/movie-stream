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

            if (response.HasError)
            {
                return Unauthorized(response.Error);
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            var response = await _accountService.RegisterBasicUserAsync(request, origin);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("register-content-manager")]
        public async Task<IActionResult> RegisterContentManagerAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            var response = await _accountService.RegisterContentManagerUserAsync(request, origin);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterRequest request)
        {
            var origin = Request.Headers.Origin;
            var response = await _accountService.RegisterAdminUserAsync(request, origin);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
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
            var response = await _accountService.ForgotPasswordAsync(request, origin);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var response = await _accountService.ResetPasswordAsync(request);

            if (response.HasError)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
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
