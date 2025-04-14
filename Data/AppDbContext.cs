using Microsoft.EntityFrameworkCore;

namespace SchedulingSystemAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        
    }
}