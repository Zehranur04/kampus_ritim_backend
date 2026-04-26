using FluentValidation;

namespace KampusRitim.Application.UseCases.UserEvent.LeaveEvent
{
    public class LeaveEventValidation : AbstractValidator<LeaveEventRequest>
    {
        public LeaveEventValidation()
        {
            RuleFor(x => x.EventId).GreaterThan(0);
        }
    }
}
