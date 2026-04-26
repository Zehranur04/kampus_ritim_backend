using KampusRitim.Application.Features.Dtos; // RecommendedClubDto burada olmalı

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedClub
{
    public class GetRecommendedClubResponse
    {
        public string CuteMessage { get; set; } = "";

        // Listeyi new() ile başlatıyoruz ki "null" hatası almayalım.
        public List<RecommendedClubDto> Clubs { get; set; } = new();
    }
}
