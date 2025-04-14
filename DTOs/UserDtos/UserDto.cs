using SchedulingSystemAPI.Models.Enums;

namespace SchedulingSystemAPI.DTOs.UserDtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool IsActive { get; set; }
    }
}