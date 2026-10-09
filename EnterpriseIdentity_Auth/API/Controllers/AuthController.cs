using EnterpriseIdentity_Auth.Application.DTOs.Account;
using EnterpriseIdentity_Auth.Application.DTOs.Auth;
using EnterpriseIdentity_Auth.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EnterpriseIdentity_Auth.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _authService.Register(dto);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var result = await _authService.Login(dto, ipAddress, userAgent);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var sessionId = User.FindFirst("session_id")?.Value;

            if (string.IsNullOrEmpty(sessionId))
            {
                return Unauthorized();
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            await _authService.Logout(Guid.Parse(sessionId), ipAddress);

            return Ok(new
            {
                message = "Logout successful"
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenDTO dto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            var result = await _authService.RefreshToken(dto.RefreshToken, ipAddress);

            return Ok(result);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke(RefreshTokenDTO dto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            await _authService.RevokeToken(dto.RefreshToken, ipAddress);

            return Ok();
        }

        [HttpPost("activate")]
        public async Task<IActionResult> Activate(ActivateAccountDTO dto)
        {
            var result = await _authService.ActivateAccount(dto);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var result = await _authService.GetUserDataById(int.Parse(userId));

            return Ok(result);
        }
    }
}
