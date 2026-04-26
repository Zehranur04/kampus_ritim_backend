using MediatR;

namespace KampusRitim.Application.UseCases.Event.UpdateEvent
{
    // Geriye UpdateEventResponse dönecek
    public class UpdateEventRequest : IRequest<UpdateEventResponse>
    {
        public int Id { get; set; } // Hangi etkinlik güncellenecek?
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Time { get; set; }
        public string Location { get; set; } = null!;
        public int Quota { get; set; }
        public string? CertificateDetails { get; set; }
        // Seçenek A: Var olanı seçerse burası dolu gelir
        public int? SpeakerId { get; set; }

        // Seçenek B: Yeni oluşturursa buralar dolu gelir
        public string? NewSpeakerName { get; set; }
        public string? NewSpeakerSurname { get; set; }
        public string? NewSpeakerTitle { get; set; }
        public string? NewSpeakerBio { get; set; }
        public string? NewSpeakerImageUrl { get; set; }
    }
}