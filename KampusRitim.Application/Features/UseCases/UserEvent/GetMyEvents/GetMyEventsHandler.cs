using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.UserEvent.GetMyEvents
{
    public class GetMyEventsHandler : IRequestHandler<GetMyEventsRequest, GetMyEventsResponse>
    {
        private readonly IUserEventRepository _userEventRepo;
        private readonly ICurrentUserService _currentUserService;

        public GetMyEventsHandler(IUserEventRepository userEventRepo, ICurrentUserService currentUserService)
        {
            _userEventRepo = userEventRepo;
            _currentUserService = currentUserService;
        }

        public async Task<GetMyEventsResponse> Handle(GetMyEventsRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                return new GetMyEventsResponse
                {
                    IsSuccess = false,
                    Message = "Oturum bulunamadı."
                };
            }

            var ids = await _userEventRepo.GetAttendingEventIdsByUserIdAsync(userId.Value);

            return new GetMyEventsResponse
            {
                IsSuccess = true,
                Message = "Katıldığınız etkinlikler getirildi.",
                EventIds = ids?.ToList() ?? new List<int>()
            };
        }
    }
}
