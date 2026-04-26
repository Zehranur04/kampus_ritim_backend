namespace KampusRitim.Domain.Entity
{
    public class Vote
    {
        public int Id { get; set; }
        public int Score { get; set; } // 1-5 arası puan
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
    }
}
