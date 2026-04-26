using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Persistence
{
    public class ClubRepository : IClubRepository
    {
        private readonly AppDbContext _context;

        public ClubRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Club>> GetAllAsync(string? searchTerm = null, int? categoryId = null)
        {
            // 1. Sorguyu oluşturmaya başla (henüz veritabanına gitmedi)
            var query = _context.Clubs.AsNoTracking().AsQueryable();

            // 2. Arama metni varsa filtrele (İsme göre)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }

            // 3. Kategori seçilmişse filtrele
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(c => c.CategoryId == categoryId.Value);
            }

            // 4. Sorguyu çalıştır ve listeyi döndür
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Club>> GetAllAsync()
        {
            // OPTİMİZASYON: Sadece okuma yapıldığı için AsNoTracking ekledik. Daha hızlıdır.
            return await _context.Clubs.AsNoTracking().ToListAsync();
        }

        public async Task<Club?> GetByIdAsync(int id)
        {
            return await _context.Clubs.FindAsync(id);
        }

        public async Task<Club> AddAsync(Club club)
        {
            await _context.Clubs.AddAsync(club);
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task<Club?> UpdateAsync(Club club)
        {
            // DÜZELTME: Tekrar FindAsync yapmaya gerek yok.
            // Handler zaten güncellenmiş entity'yi gönderiyor.
            _context.Clubs.Update(club);
            await _context.SaveChangesAsync();
            return club;
        }

        public async Task DeleteAsync(Club club)
        {
            _context.Clubs.Remove(club);
            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetMemberIdsByClubIdAsync(int clubId)
        {
            // Veritabanındaki 'UserClubs' (veya senin tablonda adı neyse) tablosuna gider.
            // O kulübe ait kayıtları bulur.
            // Sadece 'UserId'leri seçip liste olarak döner.

            return await _context.UserClubs // NOT: Buradaki tablo ismin 'ClubMembers' veya 'Members' da olabilir. Context dosyana bakıp düzeltebilirsin.
                                 .Where(x => x.ClubId == clubId)
                                 .Select(x => x.UserId)
                                 .ToListAsync();
        }
    }
}