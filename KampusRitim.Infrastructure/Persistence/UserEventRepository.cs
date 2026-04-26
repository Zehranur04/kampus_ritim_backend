using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Repositories
{
    public class UserEventRepository : IUserEventRepository
    {
        private readonly AppDbContext _context;

        public UserEventRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. Katılım Ekle
        public async Task AddAsync(UserEvent userEvent)
        {
            await _context.UserEvents.AddAsync(userEvent);
            await _context.SaveChangesAsync();
        }

        // 2. Katılım Sil (Sadece Async olanı bıraktık)
        public async Task DeleteAsync(UserEvent userEvent)
        {
            _context.UserEvents.Remove(userEvent);
            await _context.SaveChangesAsync();
        }

        // 3. Getir (Kontrol)
        public async Task<UserEvent?> GetAsync(int userId, int eventId)
        {
            return await _context.UserEvents
                .FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId);
        }

        // 4. Liste Getir (Profil için)
        public async Task<IEnumerable<int>> GetAttendingEventIdsByUserIdAsync(int userId)
        {
            return await _context.UserEvents
                .AsNoTracking() // Sadece okuma olduğu için performans artırır
                .Where(ue => ue.UserId == userId)
                .Select(ue => ue.EventId)
                .ToListAsync();
        }
    }
}