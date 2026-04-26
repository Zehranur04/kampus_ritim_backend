using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Event>> GetAllAsync(string? searchTerm = null, int? categoryId = null)
        {
            // 1. Sorguyu oluştur (Include Speaker önemli, silmiyoruz)
            var query = _context.Events
                .AsNoTracking() // Performans
                .Include(e => e.Speaker) // Konuşmacı bilgisi gerekli
                .AsQueryable();

            // 2. Arama metni varsa filtrele (Etkinlik Başlığına -Title- göre)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.Title.Contains(searchTerm));
            }

            // 3. Kategori seçilmişse filtrele
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(e => e.CategoryId == categoryId.Value);
            }

            // 4. Sonuçları getir
            return await query.ToListAsync();
        }
        public async Task<Event?> GetByIdWithParticipantsAsync(int eventId)
        {
            return await _context.Events
                .Include(e => e.UserEvents) // Katılımcı sayısını kontrol etmek için ŞART
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            // Listelemede Speaker ismini göstermek için Include ekledik
            return await _context.Events
                .AsNoTracking() // Performans için
                .Include(e => e.Speaker)
                .ToListAsync();
        }

        public async Task<Event?> GetByIdWithSpeakerAsync(int eventId)
        {
            // Detay sayfasında konuşmacı bilgisi lazım
            return await _context.Events
                .Include(e => e.Speaker)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        public async Task<Event?> GetByIdAsync(int eventId)
        {
            // Sadece var mı yok mu bakmak veya güncellemek için sade getirme
            return await _context.Events.FindAsync(eventId);
        }

        public async Task<Event> AddAsync(Event entity)
        {
            await _context.Events.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Event> UpdateAsync(Event entity)
        {
            _context.Events.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(Event eventEntity)
        {
            // Handler zaten bulup gönderdi, biz sadece siliyoruz.
            _context.Events.Remove(eventEntity);
            await _context.SaveChangesAsync();
        }
    }
}