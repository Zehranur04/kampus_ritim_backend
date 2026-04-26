using MediatR;

namespace KampusRitim.Application.Features.UseCases.GetMyClub
{
    public class GetMyClubRequest : IRequest<GetMyClubResponse>
    {
        // Parametreye gerek yok, çünkü "mevcut kullanıcıyı" isteyeceğiz.
    }
}
