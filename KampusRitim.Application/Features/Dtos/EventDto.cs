namespace KampusRitim.Application.Features.Dtos
{
    public class EventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Time { get; set; }
        public string Location { get; set; } = null!;
        public int Quota { get; set; }
        public string? CertificateDetails { get; set; }

        public string? SpeakerName { get; set; }   // Speaker (Konuşmacı) bilgisini ID olarak değil, isim olarak dönmek daha şıktır
    }
}