using KampusRitim.Application.Interfaces.Repositories; // IClubRepository'nin olduğu yer
using MediatR;

namespace KampusRitim.Application.UseCase.Club.CreateClub
{
    public class CreateClubHandler : IRequestHandler<CreateClubRequest, CreateClubResponse>
    {
        private readonly IClubRepository _clubRepo;

        public CreateClubHandler(IClubRepository clubRepo)
        {
            _clubRepo = clubRepo;
        }

        public async Task<CreateClubResponse> Handle(CreateClubRequest request, CancellationToken cancellationToken)
        {
            // 1. Request'ten gelen veriyi Entity'ye çeviriyoruz (Mapping)
            var entity = new Domain.Entity.Club
            {
                Name = request.Name,
                Description = request.Description,
                ProfileImageUrl = request.ProfileImageUrl,
                CreatedAt = DateTime.UtcNow, // Tarihi sunucu şimdiki zaman olarak atar
                CategoryId = request.CategoryId // YENİ EKLENDİ
            };

            // 2. Repository aracılığıyla veritabanına ekliyoruz
            var createdClub = await _clubRepo.AddAsync(entity);

            // 3. Oluşan Entity'yi Response nesnesine çevirip geri döndürüyoruz
            return new CreateClubResponse
            {
                Id = createdClub.Id,
                Name = createdClub.Name,
                Description = createdClub.Description,
                ProfileImageUrl = createdClub.ProfileImageUrl,
                CreatedAt = createdClub.CreatedAt
            };
        }
    }
}