namespace KampusRitim.Application.Features.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public bool IsRead { get; set; }
        public int? RelatedEventId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
