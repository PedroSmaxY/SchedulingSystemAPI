using Microsoft.IdentityModel.Tokens;
using SchedulingSystemAPI.DTOs.AuthDtos;
using SchedulingSystemAPI.DTOs.UserDtos;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Repositories.Interfaces;
using SchedulingSystemAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace SchedulingSystemAPI.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration configuration) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);

            if (user == null || !BC.Verify(loginDto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Email ou senha inválidos");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Usuário desativado");
            }

            var token = GenerateJwtToken(user);

            return new TokenDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(12),
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive
                }
            };
        }

        public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Email já está em uso");

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email,
                Password = BC.HashPassword(createUserDto.Password),
                Role = createUserDto.Role
            };

            await _userRepository.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado");

            if (!BC.Verify(currentPassword, user.Password))
                return false;

            user.Password = BC.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}