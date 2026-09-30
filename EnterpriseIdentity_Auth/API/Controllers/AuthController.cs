using EnterpriseIdentity_Auth.Application.DTOs.Auth;
using EnterpriseIdentity_Auth.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
    }
}
