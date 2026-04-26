using MediatR;

namespace KampusRitim.Application.UseCases.User.GetUserById
{
    // Tek bir kullanıcı döneceği için Response içinde UserDto taşıyacağız.
    public class GetUserByIdRequest : IRequest<GetUserByIdResponse>
    {
        public int Id { get; set; }
    }
}