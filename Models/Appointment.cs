using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchedulingSystemAPI.Models.Enums;

namespace SchedulingSystemAPI.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public Service? Service { get; set; }

        [Required]
        public int AvailableSlotId { get; set; }

        [ForeignKey("AvailableSlotId")]
        public AvailableSlot? AvailableSlot { get; set; }

        [Required]
        public DateTime AppointmentDateTime { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Pending;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public void Confirm()
        {
            if (Status == Status.Pending)
            {
                Status = Status.Confirmed;
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public void Cancel()
        {
            if (Status != Status.Canceled)
            {
                Status = Status.Canceled;
                UpdatedAt = DateTime.UtcNow;

                AvailableSlot?.Release();
            }
        }

        public Appointment(int userId, int serviceId, DateTime appointmentDateTime)
        {
            UserId = userId;
            ServiceId = serviceId;
            AppointmentDateTime = appointmentDateTime;
        }

        public Appointment() { }
    }
}