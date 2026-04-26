using MediatR;

namespace KampusRitim.Application.Features.UseCases.UserClub.LeaveClub
{
    public class LeaveClubRequest : IRequest<LeaveClubResponse>
    {
        public int ClubId { get; set; }
    }
}