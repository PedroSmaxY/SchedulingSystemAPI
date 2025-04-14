using SchedulingSystemAPI.DTOs.AuthDtos;
using SchedulingSystemAPI.DTOs.UserDtos;

namespace SchedulingSystemAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(CreateUserDto createUserDto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}