using FluentValidation;
using KampusRitim.Application.UseCases.Club.GetClubMember;

namespace KampusRitim.Application.UseCases.Club.GetClubMembers
{
    public class GetClubMemberValidation : AbstractValidator<GetClubMemberRequest>
    {
        public GetClubMemberValidation ()
        {
            RuleFor(x => x.ClubId)
                .GreaterThan(0).WithMessage("Geçersiz Kulüp ID.");
        }
    }
}