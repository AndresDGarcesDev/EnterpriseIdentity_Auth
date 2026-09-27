using EnterpriseIdentity_Auth.Application.DTOs.Auth;
using EnterpriseIdentity_Auth.Application.Interfaces;
using EnterpriseIdentity_Auth.Domain.Entities;
using EnterpriseIdentity_Auth.Infraestructure.Data;
using EnterpriseIdentity_Auth.Infraestructure.Security;

namespace EnterpriseIdentity_Auth.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly PortfAdgarcaAuthEnterpriseContext _dbContext;
        private readonly JwtHelper _jwtHelper;

        public AuthService(PortfAdgarcaAuthEnterpriseContext dbContext, JwtHelper jwtHelper)
        {
            _dbContext = dbContext;
            _jwtHelper = jwtHelper;
        }

        public async Task<string> Register(RegisterDTO dto)
        {
            var exist = _dbContext.Users.Any(u => u.Email == dto.Email);

            if (exist) throw new Exception("User already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                RolesId = dto.Role,
                IsActive = true,
                HasPassword = true,
                CreatedAt = DateTime.Now,
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return "User registered successfully";
        }
    }
}
