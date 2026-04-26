using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.Profile.GetProfileById
{
    public class GetProfileByIdResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Bulunan profil (Record formatında)
        public ProfileDto? Profiles { get; set; }
    }
}