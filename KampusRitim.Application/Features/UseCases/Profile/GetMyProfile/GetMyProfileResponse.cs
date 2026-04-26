using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.Profile.GetMyProfile
{
    public class GetMyProfileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Kullanıcının kendi profil bilgileri
        public ProfileDto? Profile { get; set; }
    }
}
