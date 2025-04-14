using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemAPI.DTOs.ServiceDtos
{
    public class UpdateServiceDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [Range(1, 999)]
        public int? Duration { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}