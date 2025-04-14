namespace SchedulingSystemAPI.DTOs.AuthDtos
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserDtos.UserDto User { get; set; } = null!;
    }
}