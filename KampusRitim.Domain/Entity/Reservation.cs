using KampusRitim.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampusRitim.Domain.Entity
{
    public class Reservation
    {
        public int Id { get; set; }

        // Who requested this reservation? (nullable for backward compatibility)
        public int? UserId { get; set; }
        public User? User { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending; // Varsayılan: Beklemede

        // Hangi Oda?
        public int RoomId { get; set; }
        public Room Room { get; set; } = null!;

        // Hangi Etkinlik için? (Event entity ile ilişki kurabiliriz veya sadece ID tutabiliriz)
        // Şimdilik basitlik adına sadece ID tutuyoruz.
        public int EventId { get; set; }
    }
}
