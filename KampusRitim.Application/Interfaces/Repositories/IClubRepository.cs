using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAllAsync(string? searchTerm = null, int? categoryId = null);
        Task<IEnumerable<Club>> GetAllAsync();     // Tüm kulüpleri listeler
        Task<Club?> GetByIdAsync(int id);         // ID'ye göre tek bir kulüp getirir

        Task<Club> AddAsync(Club club);     // Yeni kulüp ekler

        Task<Club?> UpdateAsync(Club club);           // Kulüp bilgilerini günceller

        Task DeleteAsync(Club club);          // Kulübü siler (Karar aldığımız gibi Entity alıyor, ID değil)
        Task<List<int>> GetMemberIdsByClubIdAsync(int clubId);  // Kulüp üyelerinin ID'lerini bulmak için bu metoda ihtiyacımız var

    }
}