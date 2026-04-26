using KampusRitim.Domain.Entity;
using System.Collections.Generic; // ICollection kullanmak için bu kütüphane gerekli

namespace KampusRitim.Domain.Entities
{
    public class Category
    {
        // Constructor (Yapıcı Metot): 
        // Kategori ilk oluştuğunda bu listeler boş(null değil) olarak başlasın diye bunu yapıyoruz.
        // Böylece "NullReferenceException" hatası almayız.
        public Category()
        {
            Clubs = new HashSet<Club>();
            Events = new HashSet<Event>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
    
        // Bir kategorinin içinde birden fazla Kulüp ve Etkinlik olabilir.
        public ICollection<Club> Clubs { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}