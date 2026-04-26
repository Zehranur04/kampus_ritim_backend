using FluentValidation;

namespace KampusRitim.Application.UseCases.Profile.UpdateProfile
{
    public class UpdateProfileValidation : AbstractValidator<UpdateProfileRequest>
    {
        public UpdateProfileValidation ()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50).WithMessage("Ad alanı en fazla 50 karakter olabilir.");
            RuleFor(x => x.Surname).NotEmpty().MaximumLength(50).WithMessage("Soyad alanı en fazla 50 karakter olabilir.");

            RuleFor(x => x.Bio)
                .MaximumLength(500).WithMessage("Biyografi en fazla 500 karakter olabilir.");
        }
    }
}

