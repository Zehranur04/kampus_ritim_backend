using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Persistence
{
    public class UserClubRepository : IUserClubRepository
    {
        private readonly AppDbContext _context;

        public UserClubRepository(AppDbContext context)
        {
            _context = context;
        }

        // 1. JOIN (Ekleme)
        public async Task JoinClubAsync(UserClub userClub)
        {
            // Handler'dan gelen hazır nesneyi ekliyoruz
            await _context.UserClubs.AddAsync(userClub);
            await _context.SaveChangesAsync();
        }

        // 2. LEAVE (Silme)
        public async Task LeaveClubAsync(UserClub userClub)
        {
            // Handler'ın bulup gönderdiği nesneyi siliyoruz
            _context.UserClubs.Remove(userClub);
            await _context.SaveChangesAsync();
        }

        // 3. UPDATE (Rol Güncelleme vb.)
        public async Task UpdateMemberRoleAsync(UserClub userClub)
        {
            _context.UserClubs.Update(userClub);
            await _context.SaveChangesAsync();
        }

        // --- Okuma İşlemleri (Değişmedi) ---

        public async Task<UserClub?> GetMembershipAsync(int userId, int clubId)
        {
            return await _context.UserClubs.FindAsync(userId, clubId);
        }

        public async Task<IEnumerable<UserClub>> GetMembersOfClubAsync(int clubId)
        {
            return await _context.UserClubs
                .AsNoTracking()
                .Where(uc => uc.ClubId == clubId)
                .Include(uc => uc.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserClub>> GetClubsForUserAsync(int userId)
        {
            return await _context.UserClubs
                .AsNoTracking()
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Club)
                .ToListAsync();
        }
    }
}