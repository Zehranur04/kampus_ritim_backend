using FluentValidation;

namespace KampusRitim.Application.UseCases.Club.GetClubById
{
    public class GetClubByIdValidation : AbstractValidator<GetClubByIdRequest>
    {
        public GetClubByIdValidation()
        {
            RuleFor(x => x.ClubId)
                .GreaterThan(0).WithMessage("Geçersiz Kulüp ID'si. ID 0'dan büyük olmalıdır.")
                .NotEmpty().WithMessage("Kulüp ID boş bırakılamaz.");
        }
    }
}