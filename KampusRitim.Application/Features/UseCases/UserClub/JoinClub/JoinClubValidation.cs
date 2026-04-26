using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.UserClub.JoinClub
{
    public class JoinClubValidation : AbstractValidator<JoinClubRequest>
    {
        public JoinClubValidation ()
        {

            RuleFor(x => x.ClubId)
                .GreaterThan(0).WithMessage("Geçersiz Kulüp ID.");
        }
    }
}