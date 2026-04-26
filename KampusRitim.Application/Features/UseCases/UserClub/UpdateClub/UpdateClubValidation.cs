using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.UserClub.UpdateClub
{
    public class UpdateClubValidation : AbstractValidator<UpdateClubRequest>
    {
        public UpdateClubValidation()
        {
            // ID Kontrolü
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Güncellenecek kulübün ID'si geçersiz.");

            // İsim Kontrolü (Create ile aynı kurallar)
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kulüp adı boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Kulüp adı 100 karakteri geçemez.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Açıklama 500 karakteri geçemez.");
        }
    }
}

