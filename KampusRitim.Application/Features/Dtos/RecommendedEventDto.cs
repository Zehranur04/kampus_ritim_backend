namespace KampusRitim.Application.Features.Dtos
{
    public class RecommendedEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Time { get; set; }
        public string Location { get; set; } = string.Empty;
        public int Quota { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string SpeakerName { get; set; } = string.Empty;
    }
}
