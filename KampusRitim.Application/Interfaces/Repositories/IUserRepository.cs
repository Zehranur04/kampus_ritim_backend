using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        // --- Okuma İşlemleri ---
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByEmailAsync(string email);

        // --- Yazma İşlemleri ---
        Task<User> AddAsync(User user);    // Geriye eklenen kullanıcıyı döner (ID'si oluşmuş halde)

        Task<User> UpdateAsync(User user);    // Güncellenen kullanıcıyı döner

        Task DeleteAsync(User user);   // Parametre olarak silinecek entity'yi alır (ID değil!)
    }
}