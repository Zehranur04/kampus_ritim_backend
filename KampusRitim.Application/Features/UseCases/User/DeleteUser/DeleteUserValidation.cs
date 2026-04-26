using FluentValidation;
using KampusRitim.Application.UseCases.User.DeleteUser;

namespace KampusRitim.Application.UseCases.User.DeleteUser
{
    public class DeleteUserValidation : AbstractValidator<DeleteUserRequest>
    {
        public DeleteUserValidation()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Silinecek kullanıcı ID'si geçersiz.")
                .NotEmpty().WithMessage("ID alanı boş bırakılamaz.");
        }
    }
}