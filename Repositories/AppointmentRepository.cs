using Microsoft.EntityFrameworkCore;
using SchedulingSystemAPI.Data;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Models.Enums;
using SchedulingSystemAPI.Repositories.Interfaces;

namespace SchedulingSystemAPI.Repositories
{
    public class AppointmentRepository(AppDbContext context) : IAppointmentRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.AvailableSlot)
                .ToListAsync();
        }

        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.AvailableSlot)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetByUserIdAsync(int userId)
        {
            return await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.AvailableSlot)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByServiceIdAsync(int serviceId)
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.AvailableSlot)
                .Where(a => a.ServiceId == serviceId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.AvailableSlot)
                .Where(a => a.AppointmentDateTime >= start && a.AppointmentDateTime <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<Appointment>> GetByStatusAsync(Status status)
        {
            return await _context.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.AvailableSlot)
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
        }
    }
}