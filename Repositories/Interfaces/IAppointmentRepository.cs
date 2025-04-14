using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Models.Enums;

namespace SchedulingSystemAPI.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task<IEnumerable<Appointment>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Appointment>> GetByServiceIdAsync(int serviceId);
        Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task<IEnumerable<Appointment>> GetByStatusAsync(Status status);
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);
    }
}