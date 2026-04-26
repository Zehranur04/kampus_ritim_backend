using MediatR;

namespace KampusRitim.Domain.Events
{
    // Bu sınıf, sistem içinde dolaşacak olan "bilgi paketi"dir.
    public class EventCreatedDomainEvent : INotification
    {
        public int EventId { get; }
        public string EventTitle { get; }
        public int ClubId { get; }
        public string ClubName { get; }

        public EventCreatedDomainEvent(int eventId, string eventTitle, int clubId, string clubName)
        {
            EventId = eventId;
            EventTitle = eventTitle;
            ClubId = clubId;
            ClubName = clubName;
        }
    }
}