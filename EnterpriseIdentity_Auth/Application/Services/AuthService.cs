using EnterpriseIdentity_Auth.Application.DTOs.Account;
using EnterpriseIdentity_Auth.Application.DTOs.Auth;
using EnterpriseIdentity_Auth.Application.Interfaces;
using EnterpriseIdentity_Auth.Domain.Entities;
using EnterpriseIdentity_Auth.Infraestructure.Data;
using EnterpriseIdentity_Auth.Infraestructure.Security;
using Microsoft.EntityFrameworkCore;
using UAParser;
using Volo.Abp;

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

        public async Task<AuthResponseDTO> Login(LoginDTO dto, string? ipAddress, string? userAgent)
        {
            var user = await _dbContext.Users
                            .Include(u => u.Roles)
                            .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Usuario no encontrado");

            if (!user.IsActive)
                throw new Exception("Cuenta no activada");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Credenciales inválidas");

            var parser = Parser.GetDefault();

            ClientInfo clientInfo = parser.Parse(userAgent);

            var browser = clientInfo.UA.Family;
            var os = clientInfo.OS.Family;
            var device = clientInfo.Device.Family;
            var sessionId = Guid.NewGuid().ToString();

            var accessToken = _jwtHelper.GenerateToken(user, sessionId);
            var refreshToken = _jwtHelper.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                SessionId = Guid.NewGuid(),
                Browser = browser,
                OperatingSystem = os,
                DeviceType = device,
                UserAgent = userAgent,
                LastActivity = DateTime.UtcNow,
            };

            _dbContext.RefreshTokens.Add(refreshTokenEntity);
            await _dbContext.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                Email = user.Email,
                Role = user.Roles.Name,
            };
        }

        public async Task Logout(Guid sessionId, string? ipAddress)
        {
            var session = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x =>
                    x.SessionId == sessionId &&
                    x.Revoked == null &&
                    x.Expires > DateTime.UtcNow
                );

            if (session == null)
                return;

            session.Revoked = DateTime.UtcNow;
            session.RevokedByIp = ipAddress;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<AuthResponseDTO> RefreshToken(string token, string ipAddress)
        {
            var refreshToken = _dbContext.RefreshTokens
                .FirstOrDefault(x => x.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
                throw new Exception("Invalid token");

            var user = _dbContext.Users.Find(refreshToken.UserId);
            var sessionId = Guid.NewGuid().ToString();

            var newAccessToken = _jwtHelper.GenerateToken(user, sessionId);
            var newRefreshToken = _jwtHelper.GenerateRefreshToken();

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken;

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            _dbContext.RefreshTokens.Add(newRefreshTokenEntity);

            await _dbContext.SaveChangesAsync();

            return new AuthResponseDTO
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task RevokeToken(string token, string? ipAddress)
        {
            var refreshToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            if (refreshToken == null)
                throw new Exception("Token not found");

            if (!refreshToken.IsActive)
                throw new Exception("Token inactive");

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> ActivateAccount(ActivateAccountDTO dto)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Usuario no encontrado");

            if (user.IsActive)
            {
                throw new BusinessException("La cuenta ya esta activa",
                                            "ACCOUNT_ALREADY_ACTIVE");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.IsActive = true;
            user.HasPassword = true;

            await _dbContext.SaveChangesAsync();

            return "Cuenta activada correctamente";
        }
    }
}
