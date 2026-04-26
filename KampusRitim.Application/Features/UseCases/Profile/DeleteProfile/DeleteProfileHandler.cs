using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Profile.DeleteProfile
{
    public class DeleteProfileHandler : IRequestHandler<DeleteProfileRequest, DeleteProfileResponse>
    {
        private readonly IProfileRepository _profileRepo;

        public DeleteProfileHandler(IProfileRepository profileRepo)
        {
            _profileRepo = profileRepo;
        }

        public async Task<DeleteProfileResponse> Handle(DeleteProfileRequest request, CancellationToken cancellationToken)
        {
            // 1. ADIM: Önce silinecek profili bul
            var profileToDelete = await _profileRepo.GetByIdAsync(request.Id);

            // 2. ADIM: Kontrol
            if (profileToDelete == null)
            {
                return new DeleteProfileResponse
                {
                    IsSuccess = false,
                    Message = $"Silinmek istenen {request.Id} numaralı profil bulunamadı."
                };
            }

            // 3. ADIM: Silme İşlemi (Entity veriyoruz)
            await _profileRepo.DeleteAsync(profileToDelete);

            return new DeleteProfileResponse
            {
                IsSuccess = true,
                Message = "Profil başarıyla silindi."
            };
        }
    }
}