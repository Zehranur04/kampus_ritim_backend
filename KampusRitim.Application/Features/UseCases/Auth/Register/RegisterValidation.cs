using FluentValidation;

namespace KampusRitim.Application.UseCases.Auth.Register
{
    public class RegisterValidation : AbstractValidator<RegisterRequest>
    {
        public RegisterValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir email giriniz.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.");
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
        }
    }
}