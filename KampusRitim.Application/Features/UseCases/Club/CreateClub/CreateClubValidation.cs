using FluentValidation;

namespace KampusRitim.Application.UseCase.Club.CreateClub
{
    public class CreateClubValidation : AbstractValidator<CreateClubRequest>
    {
        public CreateClubValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kulüp adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Kulüp adı 100 karakterden uzun olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama 500 karakterden uzun olamaz.");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Lütfen bir kategori seçiniz.");
        }
    }
}