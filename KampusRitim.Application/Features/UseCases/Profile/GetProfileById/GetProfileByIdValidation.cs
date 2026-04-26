using FluentValidation;

namespace KampusRitim.Application.UseCases.Profile.GetProfileById
{
    public class GetProfileByIdValidation : AbstractValidator<GetProfileByIdRequest>
    {
        public GetProfileByIdValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçersiz Profil ID. ID 0'dan büyük olmalıdır.")
                .NotEmpty().WithMessage("Profil ID boş bırakılamaz.");
        }
    }
}