using System.ComponentModel.DataAnnotations;

namespace EnterpriseIdentity_Auth.Application.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        public int Role { get; set; }

    }
}
