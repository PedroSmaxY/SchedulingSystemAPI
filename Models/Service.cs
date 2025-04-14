using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemAPI.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [Range(1, 999)]
        public required int Duration { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Service(string name, int duration, string? description)
        {
            Name = name;
            Duration = duration;
            Description = description;
        }

        public Service() { }
    }
}