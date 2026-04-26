using FluentValidation;

namespace KampusRitim.Application.UseCases.User.GetUserByEmail
{
    public class GetUserByEmailValidator : AbstractValidator<GetUserByEmailRequest>
    {
        public GetUserByEmailValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email adresi boş bırakılamaz.")
                .EmailAddress().WithMessage("Lütfen geçerli bir email formatı giriniz.");
        }
    }
}