using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedClub
{
    public class GetRecommendedClubValidation : AbstractValidator<GetRecommendedClubRequest>
    {
        public GetRecommendedClubValidation()
        {
            // Kullanıcının kutucuğa girdiği metni (UserDescription) kontrol ediyoruz.

            RuleFor(x => x.UserDescription)
                .NotEmpty()
                .WithMessage("Lütfen ilgi alanlarınızdan veya o anki modunuzdan bahsedin, bu alanı boş bırakamazsınız.")

                .MinimumLength(10)
                .WithMessage("Yapay zekanın size doğru öneri yapabilmesi için lütfen en az 10 karakterlik bir cümle yazın.")

                .MaximumLength(500)
                .WithMessage("Çok uzun bir metin girdiniz. Lütfen özetleyerek tekrar deneyin (Maksimum 500 karakter).");
        }
    }
}