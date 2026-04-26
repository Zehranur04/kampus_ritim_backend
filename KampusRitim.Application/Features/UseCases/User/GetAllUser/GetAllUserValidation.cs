using FluentValidation;

namespace KampusRitim.Application.UseCases.User.GetAllUsers
{
    public class GetAllUserValidation : AbstractValidator<GetAllUserRequest>
    {
        public GetAllUserValidation ()
        {
            // Validasyon kuralı yok (Boş istek)
        }
    }
}