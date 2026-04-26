using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Profile.GetProfileById
{
    public class GetProfileByIdHandler : IRequestHandler<GetProfileByIdRequest, GetProfileByIdResponse>
    {
        private readonly IProfileRepository _profileRepo;

        public GetProfileByIdHandler(IProfileRepository profileRepo)
        {
            _profileRepo = profileRepo;
        }

        public async Task<GetProfileByIdResponse> Handle(GetProfileByIdRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den profili çek (Include User yapılmış olmalı)
            var profile = await _profileRepo.GetByIdAsync(request.Id);

            // 2. Kontrol: Profil var mı?
            if (profile == null)
            {
                return new GetProfileByIdResponse
                {
                    IsSuccess = false,
                    Message = $"Aranan {request.Id} numaralı profil bulunamadı.",
                    Profiles = null
                };
            }

            // 3. Mapping: Entity -> UserProfileDto (Record)
            // Verileri Constructor parantezi içine sırasıyla veriyoruz.
            var userProfileDto = new ProfileDto(
                profile.User.Name,              // Name
                profile.User.Surname,           // Surname
                profile.User.Email,             // Email
                profile.User.Faculty,           // Faculty
                profile.User.Department,        // Department
                profile.Bio,                    // Bio
                profile.ProfileImageUrl,        // ProfileImageUrl
                profile.ClassLevel,             // ClassLevel (Enum)
                profile.User.UserEvents?.Count ?? 0 // AttendingEventsCount
            );

            // 4. Başarılı Dönüş
            return new GetProfileByIdResponse
            {
                IsSuccess = true,
                Message = "Profil bilgileri getirildi.",
                Profiles = userProfileDto
            };
        }
    }
}
