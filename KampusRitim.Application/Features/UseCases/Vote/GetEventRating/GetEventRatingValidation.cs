using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Vote.GetEventRating
{
    public class GetEventRatingValidation : AbstractValidator<GetEventRatingRequest>
    {
        public GetEventRatingValidation()
        {
            RuleFor(x => x.EventId)
                .NotEmpty().WithMessage("Event Id boş olamaz.")
                .GreaterThan(0).WithMessage("Geçerli bir Event Id girilmelidir.");
        }
    }
}
