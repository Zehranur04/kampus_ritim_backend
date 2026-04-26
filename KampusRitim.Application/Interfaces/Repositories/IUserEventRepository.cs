using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IUserEventRepository
    {
        // 1. Katılım Ekle (Join)
        Task AddAsync(UserEvent userEvent);

        // 2. Katılım Sil (Leave) - Sadece Async olan kalsın
        Task DeleteAsync(UserEvent userEvent);

        // 3. Kontrol İçin Getir (Zaten katılmış mı? Ayrılacağı kayıt hangisi?)
        // Not: GetByIdsAsync ile aynı işi yapar, tek isimde birleştirelim: "GetAsync"
        Task<UserEvent?> GetAsync(int userId, int eventId);

        // 4. Profil Sayfası İçin (Hangi etkinliklere katılmış?)
        // Bu metot GetMyProfileHandler içinde gerekebilir, kalsın.
        Task<IEnumerable<int>> GetAttendingEventIdsByUserIdAsync(int userId);
    }
}