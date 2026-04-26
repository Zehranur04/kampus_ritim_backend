using FluentValidation;

namespace KampusRitim.Application.UseCases.Event.UpdateEvent
{
    public class UpdateEventValidation : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Güncellenecek etkinlik ID'si geçersiz.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Etkinlik başlığı boş bırakılamaz.")
                .MaximumLength(150).WithMessage("Başlık 150 karakteri geçemez.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama alanı zorunludur.");

            RuleFor(x => x.Time)
                .GreaterThan(DateTime.Now).WithMessage("Etkinlik tarihi geçmiş bir tarih olamaz.");

            RuleFor(x => x.Quota)
                .GreaterThan(0).WithMessage("Kontenjan en az 1 olmalıdır.");

            RuleFor(x => x)
                  .Must(x => x.SpeakerId.HasValue || !string.IsNullOrEmpty(x.NewSpeakerName))
                   .WithMessage("Lütfen ya listeden bir konuşmacı seçin ya da yeni konuşmacı bilgilerini girin.");
        }
    }
  }
