using SchedulingSystemAPI.Models;

namespace SchedulingSystemAPI.Repositories.Interfaces
{
    public interface IAvailableSlotRepository
    {
        Task<IEnumerable<AvailableSlot>> GetAllAsync();
        Task<AvailableSlot?> GetByIdAsync(int id);
        Task<IEnumerable<AvailableSlot>> GetAvailableSlotsAsync();
        Task<IEnumerable<AvailableSlot>> GetByDateRangeAsync(DateTime start, DateTime end);
        Task AddAsync(AvailableSlot slot);
        Task UpdateAsync(AvailableSlot slot);
        Task DeleteAsync(int id);
        Task<IEnumerable<AvailableSlot>> GetAvailableSlotsByDateAsync(DateTime date);
    }
}