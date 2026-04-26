using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.Profile.UpdateProfile
{
    public class UpdateProfileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Güncel DTO
        public ProfileDto? Profile { get; set; }
    }
}