using MediatR;

namespace KampusRitim.Application.UseCases.UserEvent.JoinEvent
{
    public class JoinEventRequest : IRequest<JoinEventResponse>
    {
        public int EventId { get; set; }
    }
}