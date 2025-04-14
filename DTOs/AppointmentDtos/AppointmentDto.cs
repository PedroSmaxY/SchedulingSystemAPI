using SchedulingSystemAPI.DTOs.ServiceDtos;
using SchedulingSystemAPI.DTOs.UserDtos;
using SchedulingSystemAPI.Models.Enums;

namespace SchedulingSystemAPI.DTOs.AppointmentDtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserDto? User { get; set; }
        public int ServiceId { get; set; }
        public ServiceDto? Service { get; set; }
        public int AvailableSlotId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public Status Status { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}