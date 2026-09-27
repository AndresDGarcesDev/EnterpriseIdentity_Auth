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
    }
}
