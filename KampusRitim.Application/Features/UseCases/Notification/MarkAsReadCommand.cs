using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Notification
{
    public class MarkAsReadCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public MarkAsReadCommand(int id) { Id = id; }
    }

    // Handler
    public class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkAsReadHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
        {
            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null) return false;

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }
    }
}
