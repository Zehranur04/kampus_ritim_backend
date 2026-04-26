using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AppDbContext _context;

        public ProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profile>> GetAllAsync()
        {
            return await _context.Profiles
                .AsNoTracking()
                .Include(p => p.User) // User bilgisini de getir
                .ThenInclude(u => u.UserEvents) // Etkinlik sayısını hesaplayabilmek için
                .ToListAsync();
        }

        public async Task<Profile?> GetByIdAsync(int id)
        {
            return await _context.Profiles
                .Include(p => p.User)
                .ThenInclude(u => u.UserEvents)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Profile?> GetByUserIdAsync(int userId)
        {
            // GetMyProfileHandler'ın kullandığı kritik metot bu!
            return await _context.Profiles
                .Include(p => p.User)
                .ThenInclude(u => u.UserEvents)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task AddAsync(Profile profile)
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Profile profile)
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Profile profile)
        {
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }
    }
}