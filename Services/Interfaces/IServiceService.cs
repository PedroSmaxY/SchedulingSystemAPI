using SchedulingSystemAPI.DTOs.ServiceDtos;

namespace SchedulingSystemAPI.Services.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDto>> GetAllAsync(bool includeInactive = false);
        Task<ServiceDto?> GetByIdAsync(int id);
        Task<ServiceDto> CreateAsync(CreateServiceDto dto);
        Task<ServiceDto> UpdateAsync(int id, UpdateServiceDto dto);
        Task DeleteAsync(int id);
    }
}