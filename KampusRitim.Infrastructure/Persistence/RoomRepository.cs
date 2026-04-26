using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Domain.Enums;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore; // ToListAsync ve AnyAsync için BU ŞART

namespace KampusRitim.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;

        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            // _context.Set<Room>() yerine direkt property üzerinden erişim daha sağlıklıdır.
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room?> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task AddReservationAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReservationStatusAsync(int reservationId, ReservationStatus status)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation != null)
            {
                reservation.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        // 🔥 ODA ÇAKIŞMA ALGORİTMASI
        public async Task<bool> IsRoomAvailableAsync(int roomId, DateTime startTime, DateTime endTime)
        {
            // Mantık: (Mevcut.Başlangıç < Yeni.Bitiş) VE (Mevcut.Bitiş > Yeni.Başlangıç)
            // Bu formül, zaman aralıklarının kesişip kesişmediğini matematiksel olarak kanıtlar.

            bool isOccupied = await _context.Reservations
                .AnyAsync(r =>
                    r.RoomId == roomId &&
                    r.Status != ReservationStatus.Rejected && // Reddedilenler sayılmaz
                    (r.StartTime < endTime && r.EndTime > startTime)
                );

            return !isOccupied; // Dolu değilse (True) döner.
        }

        public async Task<List<Reservation>> GetPendingReservationsAsync()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Where(r => r.Status == ReservationStatus.Pending)
                .OrderBy(r => r.StartTime)
                .ToListAsync();
        }
    }
}