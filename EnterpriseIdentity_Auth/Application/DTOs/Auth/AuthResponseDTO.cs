namespace EnterpriseIdentity_Auth.Application.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
