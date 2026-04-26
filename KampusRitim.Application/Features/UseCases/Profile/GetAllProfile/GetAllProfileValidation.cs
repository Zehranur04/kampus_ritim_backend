using FluentValidation;

namespace KampusRitim.Application.UseCases.Profile.GetAllProfile
{
    public class GetAllProfilesValidation : AbstractValidator<GetAllProfileRequest>
    {
        public GetAllProfilesValidation()
        {
            // Parametre olmadığı için validasyon kuralı yok.
        }
    }
}
