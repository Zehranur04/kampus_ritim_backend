using KampusRitim.Domain.Entity;
using KampusRitim.Domain.Enums;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        // Tüm odaları listele (Dropdown için)
        Task<List<Room>> GetAllRoomsAsync();

        // Odayı ID ile bul
        Task<Room?> GetRoomByIdAsync(int id);

        // Rezervasyon Ekle
        Task AddReservationAsync(Reservation reservation);

        // Rezervasyon Durumunu Güncelle (Onayla/Reddet)
        Task UpdateReservationStatusAsync(int reservationId, ReservationStatus status);

        // 🔥 KRİTİK METOT: Oda o saatte müsait mi?
        Task<bool> IsRoomAvailableAsync(int roomId, DateTime startTime, DateTime endTime);

        // Admin paneli: onay bekleyen rezervasyonlar
        Task<List<Reservation>> GetPendingReservationsAsync();
    }
}
