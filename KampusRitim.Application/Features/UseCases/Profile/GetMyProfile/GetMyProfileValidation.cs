using FluentValidation;

namespace KampusRitim.Application.UseCases.Profile.GetMyProfile
{
    public class GetMyProfileValidation : AbstractValidator<GetMyProfileRequest>
    {
        public GetMyProfileValidation()
        {
        }
    }
}
