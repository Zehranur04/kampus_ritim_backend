using MediatR;

namespace KampusRitim.Application.Features.UseCases.UserClub.JoinClub
{
    public class JoinClubRequest : IRequest<JoinClubResponse>
    {
        public int ClubId { get; set; }
    }
}