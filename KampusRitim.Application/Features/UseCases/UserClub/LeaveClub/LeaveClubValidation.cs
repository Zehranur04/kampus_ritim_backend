using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.UserClub.LeaveClub
{
    public class LeaveClubValidation : AbstractValidator<LeaveClubRequest>
    {
        public LeaveClubValidation()
        {

            RuleFor(x => x.ClubId)
                .GreaterThan(0).WithMessage("Geçersiz Kulüp ID.");
        }
    }
}