using SchedulingSystemAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemAPI.DTOs.UserDtos
{
    public class UpdateUserDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Email { get; set; }

        public Role? Role { get; set; }

        public bool? IsActive { get; set; }
    }
}