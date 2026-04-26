using KampusRitim.Domain.Entities;

namespace KampusRitim.Domain.Entity
{
        public class Event
        {
            public int Id { get; set; }
            public string Title { get; set; } = null!;
            public string Description { get; set; } = null!;
            public DateTime Time { get; set; }
            public string Location { get; set; } = null!;
            public int Quota { get; set; }
            public string? CertificateDetails { get; set; } 


            // Planda belirtildiği gibi BE3'ün Speaker entity'sine referans
            public int SpeakerId { get; set; }
            public Speaker Speaker { get; set; } = null!;

            public int CategoryId { get; set; }
            public Category Category { get; set; } = null!;

            public int? ClubId { get; set; } // Foreign Key
            public Club? Club { get; set; }

        // Bir etkinliğe katılan tüm kullanıcı kayıtları
        public ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();
    }
}
