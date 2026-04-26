namespace KampusRitim.Domain.Entity
{
    public class UserEvent
    {
        public int UserId { get; set; }
        public int EventId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow; // Varsayılan olarak şu anı alır

        public User User { get; set; } = null!;
        public Event Event { get; set; } = null!;
    }
}