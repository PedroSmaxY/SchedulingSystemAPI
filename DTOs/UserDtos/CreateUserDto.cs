using SchedulingSystemAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemAPI.DTOs.UserDtos
{
    public class CreateUserDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; } = Role.Client;
    }
}