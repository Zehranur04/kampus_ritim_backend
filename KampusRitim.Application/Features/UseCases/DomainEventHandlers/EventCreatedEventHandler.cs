using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Domain.Events; 
using MediatR;

namespace KampusRitim.Application.DomainEventHandlers
{
    public class EventCreatedEventHandler : INotificationHandler<EventCreatedDomainEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IClubRepository _clubRepository;

        // Dependency Injection ile Repoları alıyoruz
        public EventCreatedEventHandler(INotificationRepository notificationRepository, IClubRepository clubRepository)
        {
            _notificationRepository = notificationRepository;
            _clubRepository = clubRepository;
        }

        public async Task Handle(EventCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // 1. Etkinliği yapan kulübün üye ID'lerini çek
            var memberIds = await _clubRepository.GetMemberIdsByClubIdAsync(notification.ClubId);

            // Eğer üye yoksa boşuna işlem yapma
            if (memberIds == null || !memberIds.Any()) return;

            // 2. Her üye için bir Bildirim nesnesi hazırla
            var notificationsToAdd = new List<Notification>();

            foreach (var userId in memberIds)
            {
                var newNotification = new Notification
                {
                    UserId = userId,
                    // Mesaj: "Müzik Kulübü, 'Bahar Konseri' etkinliği oluşturdu!"
                    Message = $"{notification.ClubName} kulübü, '{notification.EventTitle}' adında yeni bir etkinlik paylaştı!",
                    IsRead = false,
                    RelatedEventId = notification.EventId,
                    CreatedAt = DateTime.UtcNow
                };

                notificationsToAdd.Add(newNotification);
            }

            // 3. Hepsini tek seferde veritabanına kaydet (Performanslı yöntem)
            await _notificationRepository.AddRangeAsync(notificationsToAdd);
        }
    }
}