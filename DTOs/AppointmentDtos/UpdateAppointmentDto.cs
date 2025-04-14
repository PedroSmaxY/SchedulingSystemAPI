using SchedulingSystemAPI.Models.Enums;

namespace SchedulingSystemAPI.DTOs.AppointmentDtos
{
    public class UpdateAppointmentDto
    {
        public Status? Status { get; set; }
        public string? Notes { get; set; }
    }
}