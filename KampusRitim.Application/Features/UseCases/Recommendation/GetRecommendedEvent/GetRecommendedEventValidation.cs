using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedEvent
{
    public class GetRecommendedEventValidation : AbstractValidator<GetRecommendedEventRequest>
    {
        public GetRecommendedEventValidation()
        {
            RuleFor(x => x.UserDescription)
                .NotEmpty().WithMessage("Lütfen nasıl bir etkinlik aradığını kısaca yaz.")
                .MinimumLength(5).WithMessage("Lütfen biraz daha detay ver (En az 5 karakter).")
                .MaximumLength(500).WithMessage("Çok uzun bir metin girdiniz (Maks 500 karakter).");
        }
    }
}