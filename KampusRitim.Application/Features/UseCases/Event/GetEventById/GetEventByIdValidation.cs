using FluentValidation;

namespace KampusRitim.Application.UseCases.Event.GetEventById
{
    public class GetEventByIdValidation : AbstractValidator<GetEventByIdRequest>
    {
        public GetEventByIdValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçersiz Etkinlik ID.");
        }
    }
}