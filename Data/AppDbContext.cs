using Microsoft.EntityFrameworkCore;
using SchedulingSystemAPI.Models;

namespace SchedulingSystemAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => (e.Entity is User || e.Entity is Service) && e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is User user)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Service service)
                {
                    service.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }
    }
}