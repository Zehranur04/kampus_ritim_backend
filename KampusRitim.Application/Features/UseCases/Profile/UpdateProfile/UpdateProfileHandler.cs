using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces; // ICurrentUserService
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Profile.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileRequest, UpdateProfileResponse>
    {
        private readonly IProfileRepository _profileRepo;
        private readonly ICurrentUserService _currentUserService; // "Kim işlem yapıyor?"

        public UpdateProfileHandler(IProfileRepository profileRepo, ICurrentUserService currentUserService)
        {
            _profileRepo = profileRepo;
            _currentUserService = currentUserService;
        }

        public async Task<UpdateProfileResponse> Handle(UpdateProfileRequest request, CancellationToken cancellationToken)
        {
            // 1. Token'dan Kullanıcı ID'sini al
            var currentUserId = _currentUserService.UserId;
            if (currentUserId == null)
            {
                return new UpdateProfileResponse { IsSuccess = false, Message = "Oturum bulunamadı." };
            }

            // 2. Mevcut Profili (ve bağlı User bilgisini) çek
            // (Repository'de Include(u => u.User) olduğu için User da gelir)
            var profile = await _profileRepo.GetByUserIdAsync(currentUserId.Value);

            if (profile == null)
            {
                // Register olurken otomatik oluşması lazımdı ama yine de güvenlik kontrolü
                return new UpdateProfileResponse { IsSuccess = false, Message = "Profil bulunamadı." };
            }

            // 3. GÜNCELLEME (Mapping)

            // A) Profile Tablosu Güncellemeleri
            profile.Bio = request.Bio;
            profile.ProfileImageUrl = request.ProfileImageUrl;
            // ClassLevel profil tarafındaysa buraya, User tarafındaysa aşağıya yazılır. 
            // (Entity yapına göre Profile'da olduğunu varsayıyorum, eğer User'da ise profile.User.ClassLevel yap)
            profile.ClassLevel = request.ClassLevel;

            // B) User Tablosu Güncellemeleri (Profile üzerinden erişiyoruz)
            profile.User.Name = request.Name;
            profile.User.Surname = request.Surname;
            profile.User.Faculty = request.Faculty;
            profile.User.Department = request.Department;
            // ClassLevel User tablosundaysa: profile.User.ClassLevel = request.ClassLevel;

            // 4. KAYIT
            // Profile güncellendiğinde, ilişkili User nesnesi de takip edildiği için o da güncellenir.
            await _profileRepo.UpdateAsync(profile);

            // 5. DTO Dönüşümü (Record Constructor)
            var profileDto = new ProfileDto(
                profile.User.Name,
                profile.User.Surname,
                profile.User.Email, // Email değişmedi, eskisi dönüyor
                profile.User.Faculty,
                profile.User.Department,
                profile.Bio,
                profile.ProfileImageUrl,
                profile.ClassLevel,
                profile.User.UserEvents?.Count ?? 0
            );

            return new UpdateProfileResponse
            {
                IsSuccess = true,
                Message = "Profil başarıyla güncellendi.",
                Profile = profileDto
            };
        }
    }
}