using FluentValidation;

namespace KampusRitim.Application.UseCases.User.UpdateUser
{
    public class UpdateUserValidation : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidation()
        {
            RuleFor(x => x.Id).GreaterThan(0);

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(50);

            // Email validasyonu
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            // Şifre validasyonu (Sadece eğer şifre alanı doluysa çalışır)
            RuleFor(x => x.NewPassword)
                .MinimumLength(6).WithMessage("Yeni şifre en az 6 karakter olmalıdır.")
                .When(x => !string.IsNullOrEmpty(x.NewPassword));
        }
    }
}