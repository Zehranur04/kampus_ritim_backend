using FluentValidation;

namespace KampusRitim.Application.UseCases.Event.CreateEvent
{
    public class CreateEventValidation : AbstractValidator<CreateEventRequest>
    {
        public CreateEventValidation()
        {
            RuleFor(x => x.UserEmail)
                .EmailAddress().WithMessage("Geçerli bir mail formatı giriniz.")
                .When(x => !string.IsNullOrWhiteSpace(x.UserEmail));

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Etkinlik başlığı zorunludur.")
                .MaximumLength(150).WithMessage("Başlık 150 karakteri geçemez.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama zorunludur.");

            RuleFor(x => x.Time)
                .GreaterThan(DateTime.Now).WithMessage("Etkinlik tarihi geçmiş bir tarih olamaz.");

            RuleFor(x => x.Quota)
                .GreaterThan(0).WithMessage("Kontenjan en az 1 kişi olmalıdır.");

            RuleFor(x => x.SpeakerName).NotEmpty().WithMessage("Konuşmacı adı zorunludur.");

            RuleFor(x => x.SpeakerSurname).NotEmpty().WithMessage("Konuşmacı soyadı zorunludur.");
        }
    }
}
