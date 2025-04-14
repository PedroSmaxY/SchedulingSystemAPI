using Microsoft.EntityFrameworkCore;
using SchedulingSystemAPI.Models;

namespace SchedulingSystemAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AvailableSlot> AvailableSlots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.AvailableSlot)
                .WithOne(s => s.Appointment)
                .HasForeignKey<Appointment>(a => a.AvailableSlotId);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => (e.Entity is User || e.Entity is Service ||
                             e.Entity is Appointment || e.Entity is AvailableSlot) &&
                             e.State == EntityState.Modified);

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
                else if (entry.Entity is Appointment appointment)
                {
                    appointment.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is AvailableSlot slot)
                {
                    slot.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => (e.Entity is User || e.Entity is Service ||
                             e.Entity is Appointment || e.Entity is AvailableSlot) &&
                             e.State == EntityState.Modified);

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
                else if (entry.Entity is Appointment appointment)
                {
                    appointment.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is AvailableSlot slot)
                {
                    slot.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}