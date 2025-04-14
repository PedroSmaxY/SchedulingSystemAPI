using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Models.Enums;
using SchedulingSystemAPI.Repositories.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace SchedulingSystemAPI.Helpers
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var adminUser = await userRepository.GetByEmailAsync("admin@example.com");

            if (adminUser == null)
            {
                var admin = new User
                {
                    Name = "Admin",
                    Email = "admin@example.com",
                    Password = BC.HashPassword("Admin@123"),
                    Role = Role.Admin,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await userRepository.AddAsync(admin);
                Console.WriteLine("Administrador inicial criado: admin@example.com / Admin@123");
            }
        }
    }
}