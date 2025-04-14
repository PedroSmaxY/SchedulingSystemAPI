using System.ComponentModel.DataAnnotations;

namespace SchedulingSystemAPI.DTOs.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int AvailableSlotId { get; set; }

        public string? Notes { get; set; }
    }
}