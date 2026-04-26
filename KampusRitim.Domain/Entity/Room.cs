using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampusRitim.Domain.Entity
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;     // Örn: Konferans Salonu A
        public int Capacity { get; set; }             // Örn: 100 Kişi
        public string Location { get; set; } = null!; // Örn: Mühendislik Fakültesi 2. Kat
        public bool HasProjector { get; set; }        // Ekstra özellik (İsteğe bağlı)

        // Bir odanın birden çok rezervasyonu olabilir
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
