using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulingSystemAPI.Models
{
    public class AvailableSlot
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public bool IsAvailable { get; set; } = true;

        public int? AppointmentId { get; set; }

        public Appointment? Appointment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public void Reserve(int appointmentId)
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Este horário não está mais disponível");

            AppointmentId = appointmentId;
            IsAvailable = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Release()
        {
            AppointmentId = null;
            IsAvailable = true;
            UpdatedAt = DateTime.UtcNow;
        }

        public AvailableSlot(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public AvailableSlot() { }
    }
}