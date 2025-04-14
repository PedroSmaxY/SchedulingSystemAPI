using Microsoft.EntityFrameworkCore;
using SchedulingSystemAPI.Models;

namespace SchedulingSystemAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is User && e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                ((User)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }

            return base.SaveChanges();
        }
    }
}