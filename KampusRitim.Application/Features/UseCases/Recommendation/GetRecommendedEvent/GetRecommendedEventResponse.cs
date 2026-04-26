using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedEvent
{
    public class GetRecommendedEventResponse
    {
        public string CuteMessage { get; set; } = string.Empty;
        public List<RecommendedEventDto> Events { get; set; } = new();
    }
}