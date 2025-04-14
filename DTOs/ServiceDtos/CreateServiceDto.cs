using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemAPI.DTOs.ServiceDtos
{
    public class CreateServiceDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 999)]
        public int Duration { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }
    }
}