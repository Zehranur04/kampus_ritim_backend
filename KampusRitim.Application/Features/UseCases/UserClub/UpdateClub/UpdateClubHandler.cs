using MediatR;
using KampusRitim.Application.Interfaces.Repositories;

namespace KampusRitim.Application.Features.UseCases.UserClub.UpdateClub
{
    public class UpdateClubHandler : IRequestHandler<UpdateClubRequest, UpdateClubResponse>
    {
        private readonly IClubRepository _clubRepo;

        public UpdateClubHandler(IClubRepository clubRepo)
        {
            _clubRepo = clubRepo;
        }

        public async Task<UpdateClubResponse> Handle(UpdateClubRequest request, CancellationToken cancellationToken)
        {
            // 1. Önce veritabanında bu ID'ye sahip kulüp var mı?
            var existingClub = await _clubRepo.GetByIdAsync(request.Id);

            // 2. Eğer yoksa Hata/Başarısız dönüyoruz (Null dönmek yerine bu daha güvenli)
            if (existingClub == null)
            {
                return new UpdateClubResponse
                {
                    IsSuccess = false,
                    Message = $"Güncellenmek istenen {request.Id} numaralı kulüp bulunamadı."
                };
            }

            // 3. Varsa, Request'ten gelen yeni verileri Entity'ye aktar
            existingClub.Name = request.Name;
            existingClub.Description = request.Description;
            existingClub.ProfileImageUrl = request.ProfileImageUrl;
            // CreatedAt güncellenmez, o yüzden dokunmuyoruz.

            // 4. Repository üzerinden güncelleme işlemini yap
            // Not: Repository içindeki SaveChangesAsync sayesinde işlem db'ye yansır.
            await _clubRepo.UpdateAsync(existingClub);

            // 5. Başarılı sonucunu ve yeni veriyi döndür
            return new UpdateClubResponse
            {
                IsSuccess = true,
                Message = "Kulüp başarıyla güncellendi.",
                Id = existingClub.Id,
                Name = existingClub.Name,
                Description = existingClub.Description,
                ProfileImageUrl = existingClub.ProfileImageUrl,
                CreatedAt = existingClub.CreatedAt
            };
        }
    }
}