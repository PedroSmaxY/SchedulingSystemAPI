using SchedulingSystemAPI.Models;

namespace SchedulingSystemAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(bool includeInactive = false);
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task PermanentDeleteAsync(int id);
    }
}

