using FluentValidation;

namespace KampusRitim.Application.UseCases.UserEvent.JoinEvent
{
    public class JoinEventValidation : AbstractValidator<JoinEventRequest>
    {
        public JoinEventValidation()
        {
            RuleFor(x => x.EventId).GreaterThan(0).WithMessage("Geçersiz Etkinlik ID.");
        }
    }
}
