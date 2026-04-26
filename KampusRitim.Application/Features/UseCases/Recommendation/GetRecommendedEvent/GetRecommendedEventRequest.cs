using MediatR;

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedEvent
{
    public class GetRecommendedEventRequest : IRequest<GetRecommendedEventResponse>
    {
        // Kullanıcı promptu: "Haftasonu boşum, müzikli bir şeyler var mı?"
        public string UserDescription { get; set; } = string.Empty;
    }
}