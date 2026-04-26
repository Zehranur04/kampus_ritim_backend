using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        // Okuma İşlemleri
        Task<IEnumerable<Profile>> GetAllAsync();
        Task<Profile?> GetByIdAsync(int id);

        // Kullanıcı ID'sine göre profil getirmek için (GetMyProfile için şart)
        Task<Profile?> GetByUserIdAsync(int userId);

        // Yazma İşlemleri
        Task AddAsync(Profile profile);
        Task UpdateAsync(Profile profile);
        Task DeleteAsync(Profile profile);
    }
}