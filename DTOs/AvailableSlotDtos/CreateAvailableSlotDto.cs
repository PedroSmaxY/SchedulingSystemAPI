using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemAPI.DTOs.AvailableSlotDtos
{
    public class CreateAvailableSlotDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}