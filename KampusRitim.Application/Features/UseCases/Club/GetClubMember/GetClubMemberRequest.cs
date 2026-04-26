using MediatR;

namespace KampusRitim.Application.UseCases.Club.GetClubMember
{
    public class GetClubMemberRequest : IRequest<GetClubMemberResponse>
    {
        public int ClubId { get; set; }
    }
}