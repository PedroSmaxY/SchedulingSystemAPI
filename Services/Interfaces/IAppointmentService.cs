using SchedulingSystemAPI.DTOs.AppointmentDtos;
using SchedulingSystemAPI.Models.Enums;

namespace SchedulingSystemAPI.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<AppointmentDto>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<AppointmentDto>> GetByStatusAsync(Status status);
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
        Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto);
        Task CancelAsync(int id);
        Task ConfirmAsync(int id);
    }
}