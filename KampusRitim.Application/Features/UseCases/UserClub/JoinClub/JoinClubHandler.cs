using KampusRitim.Application.Features.UseCases.UserClub.JoinClub;
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Application.Interfaces; // ICurrentUserService için
using KampusRitim.Domain.Entity;
using KampusRitim.Domain.Enums;
using MediatR;

namespace KampusRitim.Application.UseCases.UserClub.JoinClub
{
    public class JoinClubHandler : IRequestHandler<JoinClubRequest, JoinClubResponse>
    {
        private readonly IUserClubRepository _userClubRepo;
        private readonly ICurrentUserService _currentUserService;

        public JoinClubHandler(IUserClubRepository userClubRepo, ICurrentUserService currentUserService)
        {
            _userClubRepo = userClubRepo;
            _currentUserService = currentUserService;
        }

        public async Task<JoinClubResponse> Handle(JoinClubRequest request, CancellationToken cancellationToken)
        {
            // 1. Token'dan User ID alınıyor
            var userId = _currentUserService.UserId;

            if (userId == null)
            {
                return new JoinClubResponse { IsSuccess = false, Message = "Kullanıcı kimliği doğrulanamadı." };
            }

            // 2. Kontrol: Zaten üye mi?
            var existingMembership = await _userClubRepo.GetMembershipAsync(userId.Value, request.ClubId);

            if (existingMembership != null)
            {
                return new JoinClubResponse { IsSuccess = false, Message = "Zaten bu kulübe üyesiniz." };
            }

            // 3. Yeni Üyelik Kaydı
            var newMembership = new KampusRitim.Domain.Entity.UserClub
            {
                UserId = userId.Value,
                ClubId = request.ClubId,
                ClubRole = ClubRole.Member,
                JoinDate = DateTime.UtcNow
            };

            await _userClubRepo.JoinClubAsync(newMembership);

            return new JoinClubResponse { IsSuccess = true, Message = "Kulübe başarıyla katıldınız." };
        }
    }
}