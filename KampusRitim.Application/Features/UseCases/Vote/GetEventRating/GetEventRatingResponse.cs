namespace KampusRitim.Application.Features.UseCases.Vote.GetEventRating
{
    public class GetEventRatingResponse
    {
        public double AverageScore { get; set; } // Örn: 4.2
        public int VoteCount { get; set; }       // Örn: 15 kişi oy verdi
    }
}
