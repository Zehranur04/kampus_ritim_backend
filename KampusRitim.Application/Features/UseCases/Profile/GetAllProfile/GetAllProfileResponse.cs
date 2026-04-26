using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.Profile.GetAllProfile
{
    public class GetAllProfileResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Record yapısındaki DTO listesi
        public IEnumerable<ProfileDto> Profiles { get; set; } = new List<ProfileDto>();
    }
}