using FluentValidation;

namespace KampusRitim.Application.UseCases.Event.DeleteEvent
{
    public class DeleteEventValidation : AbstractValidator<DeleteEventRequest>
    {
        public DeleteEventValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Silinecek etkinliğin ID'si geçersiz.")
                .NotEmpty().WithMessage("ID alanı boş bırakılamaz.");
        }
    }
}