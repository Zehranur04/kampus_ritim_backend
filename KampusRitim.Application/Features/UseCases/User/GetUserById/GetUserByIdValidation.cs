using FluentValidation;

namespace KampusRitim.Application.UseCases.User.GetUserById
{
    public class GetUserByIdValidation : AbstractValidator<GetUserByIdRequest>
    {
        public GetUserByIdValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçersiz Kullanıcı ID. ID 0'dan büyük olmalıdır.")
                .NotEmpty().WithMessage("Kullanıcı ID boş bırakılamaz.");
        }
    }
}
