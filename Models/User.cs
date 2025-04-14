using System.ComponentModel.DataAnnotations;
using SchedulingSystemAPI.Models.Enums;

namespace SchedulingSystemAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public required string Email { get; set; }

        [Required]
        [StringLength(255)]
        public required string Password { get; set; }

        [Required]
        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public User(string name, string email, string password, Role role)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        public User() { }
    }
}