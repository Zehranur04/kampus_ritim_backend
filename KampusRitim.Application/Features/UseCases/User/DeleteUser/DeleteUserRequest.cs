using MediatR;

namespace KampusRitim.Application.UseCases.User.DeleteUser
{
    public class DeleteUserRequest : IRequest<DeleteUserResponse>
    {
        public int Id { get; set; }
    }
}