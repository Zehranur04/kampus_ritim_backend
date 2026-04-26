using FluentValidation;

namespace KampusRitim.Application.UseCases.Profile.DeleteProfile
{
    public class DeleteProfileValidation : AbstractValidator<DeleteProfileRequest>
    {
        public DeleteProfileValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Silinecek profil ID'si geçersiz.")
                .NotEmpty().WithMessage("ID alanı boş bırakılamaz.");
        }
    }
}