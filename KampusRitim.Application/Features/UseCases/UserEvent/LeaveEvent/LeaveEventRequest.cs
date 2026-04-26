using MediatR;

namespace KampusRitim.Application.UseCases.UserEvent.LeaveEvent
{
    public class LeaveEventRequest : IRequest<LeaveEventResponse>
    {
        public int EventId { get; set; }
    }
}