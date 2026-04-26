using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Application.UseCases.Club.GetClubMember;
using MediatR;

namespace KampusRitim.Application.UseCases.Club.GetClubMembers
{
    public class GetClubMembersHandler : IRequestHandler<GetClubMemberRequest, GetClubMemberResponse>
    {
        // Üye verisi çekeceğimiz için UserClubRepository kullanıyoruz
        private readonly IUserClubRepository _userClubRepo;

        public GetClubMembersHandler(IUserClubRepository userClubRepo)
        {
            _userClubRepo = userClubRepo;
        }

        public async Task<GetClubMemberResponse> Handle(GetClubMemberRequest request, CancellationToken cancellationToken)
        {
            // 1. Repository'den üyeleri çek (Include(u => u.User) repository içinde yapılmış olmalı)
            var members = await _userClubRepo.GetMembersOfClubAsync(request.ClubId);

            // 2. Eğer hiç üye yoksa (veya kulüp yoksa) boş liste dön
            if (members == null || !members.Any())
            {
                return new GetClubMemberResponse
                {
                    IsSuccess = true, // İşlem başarılı ama sonuç boş
                    Message = "Bu kulübe kayıtlı üye bulunamadı.",
                    Members = new List<ClubMemberDto>()
                };
            }

            // 3. Entity -> DTO Dönüştürme (Senin yazdığın mantığın aynısı)
            var memberDtos = members.Select(uc => new ClubMemberDto(
                uc.UserId,
                uc.User.Name + " " + uc.User.Surname, // Ad Soyad birleştirme
                uc.User.Email,
                uc.ClubRole.ToString(),               // Enum -> String
                uc.JoinDate
            )).ToList();

            return new GetClubMemberResponse
            {
                IsSuccess = true,
                Message = "Üyeler listelendi.",
                Members = memberDtos
            };
        }
    }
}