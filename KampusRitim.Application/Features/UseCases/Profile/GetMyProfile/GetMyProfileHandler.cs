using KampusRitim.Application.Features.Dtos;
using System.IO;
using KampusRitim.Application.Interfaces; // ICurrentUserService
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Profile.GetMyProfile
{
    public class GetMyProfileHandler : IRequestHandler<GetMyProfileRequest, GetMyProfileResponse>
    {
        private readonly IProfileRepository _profileRepo;
        private readonly ICurrentUserService _currentUserService; // Token okuyucumuz

        public GetMyProfileHandler(IProfileRepository profileRepo, ICurrentUserService currentUserService)
        {
            _profileRepo = profileRepo;
            _currentUserService = currentUserService;
        }

        public async Task<GetMyProfileResponse> Handle(GetMyProfileRequest request, CancellationToken cancellationToken)
        {
            // 1. ADIM: Token'dan "Ben kimim?" sorusunu cevapla
            var currentUserId = _currentUserService.UserId;

            // Debug: write current user id to file for troubleshooting
            try
            {
                var logPath = Path.Combine(AppContext.BaseDirectory, "getmyprofile_debug.log");
                var msg = $"[{DateTime.UtcNow:O}] CurrentUserId: {currentUserId}\n";
                File.AppendAllText(logPath, msg);
            }
            catch
            {
                // ignore logging errors
            }

            // Eğer token yoksa veya geçersizse
            if (currentUserId == null)
            {
                return new GetMyProfileResponse
                {
                    IsSuccess = false,
                    Message = "Kullanıcı oturumu bulunamadı. Lütfen giriş yapın."
                };
            }

            // 2. ADIM: Repository'den benim profilimi getir
            // (Register sırasında otomatik oluşturduğumuz için %99 vardır)
            var profile = await _profileRepo.GetByUserIdAsync(currentUserId.Value);

            if (profile == null)
            {
                // Çok nadir bir durum (Veritabanı tutarsızlığı) ama kontrol etmekte fayda var
                return new GetMyProfileResponse
                {
                    IsSuccess = false,
                    Message = "Profil bilgisine ulaşılamadı."
                };
            }

            // 3. ADIM: Entity -> DTO (Record) Dönüşümü
            // Record constructor'ına sırayla verileri veriyoruz.
            var profileDto = new ProfileDto(
                profile.User.Name,              // Name
                profile.User.Surname,           // Surname
                profile.User.Email,             // Email
                profile.User.Faculty,           // Faculty
                profile.User.Department,        // Department
                profile.Bio,                    // Bio
                profile.ProfileImageUrl,        // ProfileImageUrl
                profile.ClassLevel,             // ClassLevel (Enum)
                profile.User.UserEvents?.Count ?? 0 // Katıldığı etkinlik sayısı
            );

            return new GetMyProfileResponse
            {
                IsSuccess = true,
                Message = "Profil bilgileri başarıyla getirildi.",
                Profile = profileDto
            };
        }
    }
}
