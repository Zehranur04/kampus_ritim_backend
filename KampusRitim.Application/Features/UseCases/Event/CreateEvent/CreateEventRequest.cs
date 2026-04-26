using MediatR;

namespace KampusRitim.Application.UseCases.Event.CreateEvent
{
    public class CreateEventRequest : IRequest<CreateEventResponse>
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Time { get; set; }
        public string Location { get; set; } = null!;
        public int Quota { get; set; }
        public string? CertificateDetails { get; set; }


        public int CategoryId { get; set; }


        // --- Konuşmacı Bilgileri (YENİ) ---
        public string SpeakerName { get; set; } = null!;
        public string SpeakerSurname { get; set; } = null!;
        public string? SpeakerBio { get; set; }
        public string? SpeakerTitle { get; set; } // Ünvan (Prof. Dr. vb.)
        public string? SpeakerImageUrl { get; set; }


        public int ClubId { get; set; }        // Etkinliği düzenleyen kulübün ID'si
        public string? ClubName { get; set; }   // Bildirim mesajı için kulübün adı
    }
}