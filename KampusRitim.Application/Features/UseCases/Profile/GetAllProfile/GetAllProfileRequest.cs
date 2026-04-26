using MediatR;

namespace KampusRitim.Application.UseCases.Profile.GetAllProfile
{
    // Geriye GetAllProfilesResponse dönecek
    public class GetAllProfileRequest : IRequest<GetAllProfileResponse>
    {
        // Filtreleme yok, tüm listeyi istiyoruz.
    }
}
