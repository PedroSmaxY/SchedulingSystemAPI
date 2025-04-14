using SchedulingSystemAPI.Models;

namespace SchedulingSystemAPI.Repositories.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task<Service?> GetByNameAsync(string name);
        Task AddAsync(Service service);
        Task UpdateAsync(Service service);
        Task DeleteAsync(int id);
        Task<IEnumerable<Service>> GetActiveServicesAsync();
    }
}

