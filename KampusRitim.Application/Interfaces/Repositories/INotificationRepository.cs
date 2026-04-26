using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        // 1. Veritabanına tek bir bildirim eklemek için
        Task AddAsync(Notification notification);

        // 2. Çoklu bildirim eklemek için (Toplu gönderimlerde performans sağlar)
        Task AddRangeAsync(List<Notification> notifications);

        // 3. Bir kullanıcının bildirimlerini listelemek için
        Task<List<Notification>> GetByUserIdAsync(int userId);

        // 4. ID'ye göre bildirimi bulmak için (Örn: Okundu yapmak için çağırılır)
        // Bulamazsa null dönebilir, o yüzden '?' koyduk.
        Task<Notification?> GetByIdAsync(int id);

        // 5. Bildirimi güncellemek için (IsRead = true yapınca kaydetmek için)
        Task UpdateAsync(Notification notification);
    }
}
