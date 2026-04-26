using MediatR;

namespace KampusRitim.Application.UseCases.Profile.GetMyProfile
{
    // Parametre almadığı için içi boş, ama Response tipini belirtiyoruz.
    public class GetMyProfileRequest : IRequest<GetMyProfileResponse>
    {
    }
}