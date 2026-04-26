using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // --- OKUMA (READ) ---

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // OPTİMİZASYON: Sadece okuma yapıldığı için AsNoTracking kullandık.
            // Bu sayede EF Core verileri takip etmez, liste çok daha hızlı gelir.
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            // Büyük/küçük harf duyarlılığı olmadan arama yapar
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        // --- YAZMA (WRITE) ---

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user; // ID'si dolmuş nesneyi döner
        }

        public async Task<User> UpdateAsync(User user)
        {
            // Modern EF Core güncelleme yöntemi
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            // Handler zaten kontrolü yaptı, biz sadece siliyoruz.
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}