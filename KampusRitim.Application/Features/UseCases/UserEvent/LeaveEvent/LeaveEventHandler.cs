using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.UserEvent.LeaveEvent
{
    public class LeaveEventHandler : IRequestHandler<LeaveEventRequest, LeaveEventResponse>
    {
        private readonly IUserEventRepository _userEventRepo;
        private readonly ICurrentUserService _currentUserService;

        public LeaveEventHandler(IUserEventRepository userEventRepo, ICurrentUserService currentUserService)
        {
            _userEventRepo = userEventRepo;
            _currentUserService = currentUserService;
        }

        public async Task<LeaveEventResponse> Handle(LeaveEventRequest request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcı Kim?
            var userId = _currentUserService.UserId;
            if (userId == null) return new LeaveEventResponse { IsSuccess = false, Message = "Oturum bulunamadı." };

            // 2. Kayıt Var mı?
            var existing = await _userEventRepo.GetAsync(userId.Value, request.EventId);

            if (existing == null)
            {
                return new LeaveEventResponse { IsSuccess = false, Message = "Bu etkinliğe zaten katılmıyorsunuz." };
            }

            // 3. Sil
            await _userEventRepo.DeleteAsync(existing);

            return new LeaveEventResponse { IsSuccess = true, Message = "Etkinlikten ayrıldınız." };
        }
    }
}