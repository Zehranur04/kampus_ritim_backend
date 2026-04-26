using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Notification
{
    // Query
    public class GetUserNotificationsQuery : IRequest<List<NotificationDto>> { }

    // Handler
    public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository, ICurrentUserService currentUserService)
        {
            _notificationRepository = notificationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<List<NotificationDto>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId == null) return new List<NotificationDto>();

            var notifications = await _notificationRepository.GetByUserIdAsync(userId.Value);

            // Mapping (Entity -> DTO)
            return notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                IsRead = n.IsRead,
                RelatedEventId = n.RelatedEventId,
                CreatedAt = n.CreatedAt
            }).ToList();
        }
    }
}
