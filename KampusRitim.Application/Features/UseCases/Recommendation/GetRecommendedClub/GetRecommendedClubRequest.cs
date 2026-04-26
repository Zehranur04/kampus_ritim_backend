using MediatR;

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedClub
{
    public class GetRecommendedClubRequest : IRequest<GetRecommendedClubResponse>
    {
        // "string.Empty" diyerek varsayılan değer atıyoruz, sarı uyarı gidiyor.
        // Bu değişken Frontend'deki "Arama Kutusu"ndan gelen yazıdır.
        public string UserDescription { get; set; } = string.Empty;
    }
}