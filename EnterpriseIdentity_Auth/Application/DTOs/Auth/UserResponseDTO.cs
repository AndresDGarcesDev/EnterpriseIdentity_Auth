namespace EnterpriseIdentity_Auth.Application.DTOs.Auth
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public bool IsActive { get; set; }
    }
}
