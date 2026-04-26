using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.UserClub.LeaveClub // Namespace'i kendi yapına göre kontrol et
{
    public class LeaveClubHandler : IRequestHandler<LeaveClubRequest, LeaveClubResponse>
    {
        private readonly IUserClubRepository _userClubRepo;
        private readonly ICurrentUserService _currentUserService;

        public LeaveClubHandler(IUserClubRepository userClubRepo, ICurrentUserService currentUserService)
        {
            _userClubRepo = userClubRepo;
            _currentUserService = currentUserService;
        }

        public async Task<LeaveClubResponse> Handle(LeaveClubRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                return new LeaveClubResponse { IsSuccess = false, Message = "Oturum hatası." };

            // 1. Önce böyle bir üyelik var mı kontrol et
            var membership = await _userClubRepo.GetMembershipAsync(userId.Value, request.ClubId);

            if (membership == null)
            {
                return new LeaveClubResponse { IsSuccess = false, Message = "Bu kulübe zaten üye değilsiniz." };
            }

            // 2. Üyeliği sil (Repository'de bu metodun olduğundan emin ol)
            await _userClubRepo.LeaveClubAsync(membership);

            return new LeaveClubResponse { IsSuccess = true, Message = "Kulüpten başarıyla ayrıldınız." };
        }
    }
}