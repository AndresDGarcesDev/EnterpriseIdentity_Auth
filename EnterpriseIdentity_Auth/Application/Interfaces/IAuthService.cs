using EnterpriseIdentity_Auth.Application.DTOs.Auth;

namespace EnterpriseIdentity_Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTO dto);
    }
}
