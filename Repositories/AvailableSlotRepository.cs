using Microsoft.EntityFrameworkCore;
using SchedulingSystemAPI.Data;
using SchedulingSystemAPI.Models;
using SchedulingSystemAPI.Repositories.Interfaces;

namespace SchedulingSystemAPI.Repositories
{
    public class AvailableSlotRepository : IAvailableSlotRepository
    {
        private readonly AppDbContext _context;

        public AvailableSlotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AvailableSlot>> GetAllAsync()
        {
            return await _context.AvailableSlots.ToListAsync();
        }

        public async Task<AvailableSlot?> GetByIdAsync(int id)
        {
            return await _context.AvailableSlots.FindAsync(id);
        }

        public async Task<IEnumerable<AvailableSlot>> GetAvailableSlotsAsync()
        {
            return await _context.AvailableSlots
                .Where(s => s.IsAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<AvailableSlot>> GetByDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.AvailableSlots
                .Where(s => s.StartTime >= start && s.EndTime <= end)
                .ToListAsync();
        }

        public async Task<IEnumerable<AvailableSlot>> GetAvailableSlotsByDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            return await _context.AvailableSlots
                .Where(s => s.StartTime >= startOfDay && s.StartTime <= endOfDay && s.IsAvailable)
                .ToListAsync();
        }

        public async Task AddAsync(AvailableSlot slot)
        {
            await _context.AvailableSlots.AddAsync(slot);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AvailableSlot slot)
        {
            _context.AvailableSlots.Update(slot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var slot = await _context.AvailableSlots.FindAsync(id);
            if (slot != null)
            {
                _context.AvailableSlots.Remove(slot);
                await _context.SaveChangesAsync();
            }
        }
    }
}