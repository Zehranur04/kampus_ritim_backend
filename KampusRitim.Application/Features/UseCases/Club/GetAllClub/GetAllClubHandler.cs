using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCase.Club.GetAllClub
{
    public class GetAllClubHandler : IRequestHandler<GetAllClubRequest, List<GetAllClubResponse>>
    {
        private readonly IClubRepository _clubRepo;

        public GetAllClubHandler(IClubRepository clubRepo)
        {
            _clubRepo = clubRepo;
        }

        public async Task<List<GetAllClubResponse>> Handle(GetAllClubRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den tüm veriyi çek -- var clubs = await _clubRepo.GetAllAsync();

            var clubs = await _clubRepo.GetAllAsync(request.SearchTerm, request.CategoryId);

            // 2. Entity listesini Response listesine çevir (Mapping)
            // Select linq metodu ile tek tek dönüşüm yapıyoruz.
            var responseList = clubs.Select(c => new GetAllClubResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProfileImageUrl = c.ProfileImageUrl,
                CreatedAt = c.CreatedAt
            }).ToList();

            // 3. Listeyi döndür
            return responseList;
        }
    }
}