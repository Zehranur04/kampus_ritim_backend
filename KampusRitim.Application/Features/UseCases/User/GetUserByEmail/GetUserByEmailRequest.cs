using MediatR;

namespace KampusRitim.Application.UseCases.User.GetUserByEmail
{
    public class GetUserByEmailRequest : IRequest<GetUserByEmailResponse>
    {
        public string Email { get; set; } = null!;
    }
}