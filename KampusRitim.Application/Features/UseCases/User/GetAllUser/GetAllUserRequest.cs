using MediatR;

namespace KampusRitim.Application.UseCases.User.GetAllUsers
{
    // Cevap olarak GetAllUsersResponse dönecek
    public class GetAllUserRequest : IRequest<GetAllUserResponse>
    {
        // Parametre yok
    }
}