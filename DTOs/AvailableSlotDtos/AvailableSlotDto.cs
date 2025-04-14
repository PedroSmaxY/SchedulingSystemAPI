namespace SchedulingSystemAPI.DTOs.AvailableSlotDtos
{
    public class AvailableSlotDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsAvailable { get; set; }
        public int? AppointmentId { get; set; }
    }
}