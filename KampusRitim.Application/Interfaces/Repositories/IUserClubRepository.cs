using KampusRitim.Domain.Entity;
using KampusRitim.Domain.Enums; // Eğer enum lazımsa

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IUserClubRepository
    {
        // --- Yazma İşlemleri (İsimler Senin İstediğin Gibi) ---

        // Kulübe üye ekle (Parametre olarak Entity alır)
        Task JoinClubAsync(UserClub userClub);

        // Kulüpten üye çıkarma (Parametre olarak silinecek Entity alır)
        Task LeaveClubAsync(UserClub userClub);

        // Üyenin rolünü güncelleme
        Task UpdateMemberRoleAsync(UserClub userClub);


        // --- Okuma İşlemleri (Aynen Kalıyor) ---

        // Belli bir üyelik var mı diye kontrol etme
        Task<UserClub?> GetMembershipAsync(int userId, int clubId);

        // Bir kulübün tüm üyelerini getirme
        Task<IEnumerable<UserClub>> GetMembersOfClubAsync(int clubId);

        // Bir kullanıcının tüm kulüplerini getirme
        Task<IEnumerable<UserClub>> GetClubsForUserAsync(int userId);
    }
}