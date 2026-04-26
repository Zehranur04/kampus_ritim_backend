using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Profile.GetAllProfile
{
    public class GetAllProfileHandler : IRequestHandler<GetAllProfileRequest, GetAllProfileResponse>
    {
        private readonly IProfileRepository _profileRepo;

        public GetAllProfileHandler(IProfileRepository profileRepo)
        {
            _profileRepo = profileRepo;
        }

        public async Task<GetAllProfileResponse> Handle(GetAllProfileRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den tüm profilleri (User bilgisi dahil) çek
            var profiles = await _profileRepo.GetAllAsync();

            // 2. Mapping: Entity -> UserProfileDto (Record)
            // Record olduğu için verileri Constructor (...) içine sırasıyla veriyoruz.
            var profileDtos = profiles.Select(p => new ProfileDto(
                p.User.Name,                    // Name
                p.User.Surname,                 // Surname
                p.User.Email,                   // Email
                p.User.Faculty,                 // Faculty
                p.User.Department,              // Department
                p.Bio,                          // Bio
                p.ProfileImageUrl,              // ProfileImageUrl
                p.ClassLevel,                   // ClassLevel (Enum)
                p.User.UserEvents?.Count ?? 0   // AttendingEventsCount
            )).ToList();

            // 3. Response Dön
            return new GetAllProfileResponse
            {
                IsSuccess = true,
                Message = "Tüm profiller başarıyla listelendi.",
                Profiles = profileDtos
            };
        }
    }
}