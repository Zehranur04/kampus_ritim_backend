namespace KampusRitim.Domain.Entity
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Bildirim kime gidecek?
        public string Message { get; set; } = string.Empty; // Örn: "Satranç Kulübü yeni etkinlik paylaştı!"
        public bool IsRead { get; set; } = false; // Varsayılan olarak okunmamış gelir

        public int? RelatedEventId { get; set; } // Tıklayınca hangi etkinliğe gidecek? (Opsiyonel)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public User User { get; set; } = null!;
    }
}
