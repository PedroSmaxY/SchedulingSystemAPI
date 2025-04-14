using SchedulingSystemAPI.DTOs.AvailableSlotDtos;

namespace SchedulingSystemAPI.Services.Interfaces
{
    public interface IAvailableSlotService
    {
        Task<IEnumerable<AvailableSlotDto>> GetAllAsync();
        Task<AvailableSlotDto?> GetByIdAsync(int id);
        Task<IEnumerable<AvailableSlotDto>> GetAvailableSlotsAsync();
        Task<IEnumerable<AvailableSlotDto>> GetByDateAsync(DateTime date);
        Task<AvailableSlotDto> CreateAsync(CreateAvailableSlotDto dto);
        Task DeleteAsync(int id);
    }
}