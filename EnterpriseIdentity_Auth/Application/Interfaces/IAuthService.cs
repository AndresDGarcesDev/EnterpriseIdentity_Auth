using EnterpriseIdentity_Auth.Application.DTOs.Auth;

namespace EnterpriseIdentity_Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTO dto);
        Task<AuthResponseDTO> Login(LoginDTO dto, string? ipAddress, string? userAgent);
        Task Logout(Guid sessionId, string? ipAddress);
        Task<AuthResponseDTO> RefreshToken(string token, string ipAdress);
        Task RevokeToken(string token, string ipAddress);
    }
}
