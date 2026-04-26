using FluentValidation;

namespace KampusRitim.Application.UseCases.Club.DeleteClub
{
    public class DeleteClubValidation : AbstractValidator<DeleteClubRequest>
    {
        public DeleteClubValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Silinecek kulübün ID'si geçersiz.")
                .NotEmpty().WithMessage("ID alanı boş bırakılamaz.");
        }
    }
}