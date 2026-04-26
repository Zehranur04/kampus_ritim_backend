namespace KampusRitim.Application.Features.Dtos
{
    public class VoteDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }   // Oy vereni görmek için
        public int Score { get; set; }
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

    }
}
