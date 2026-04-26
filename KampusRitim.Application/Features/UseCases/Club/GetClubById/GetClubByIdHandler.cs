using MediatR;
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Application.Features.Dtos; 

namespace KampusRitim.Application.UseCases.Club.GetClubById
{
    public class GetClubByIdHandler : IRequestHandler<GetClubByIdRequest, GetClubByIdResponse>
    {
        private readonly IClubRepository _clubRepository;

        public GetClubByIdHandler(IClubRepository clubRepository)
        {
            _clubRepository = clubRepository;
        }

        public async Task<GetClubByIdResponse> Handle(GetClubByIdRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den veriyi soruyoruz
            var club = await _clubRepository.GetByIdAsync(request.ClubId);

            // 2. KONTROL: "Böyle bir kulüp var mı?" (İstediğin mesajı burada veriyoruz)
            if (club == null)
            {
                // try-catch olmadan, sadece mantıksal kontrolle hatayı dönüyoruz.
                return new GetClubByIdResponse
                {
                    IsSuccess = false,
                    Message = $"Aradığınız {request.ClubId} numaralı kulüp bulunamadı.",
                    Club = null
                };
            }

            // 3. Kulüp varsa DTO'ya çevirip başarıyla dönüyoruz
            var clubDto = new ClubDto(
                club.Id,
                club.Name,
                club.Description,
                club.ProfileImageUrl,
                club.CreatedAt
            );

            return new GetClubByIdResponse
            {
                IsSuccess = true,
                Message = "Kulüp başarıyla getirildi.",
                Club = clubDto
            };
        }
    }
}