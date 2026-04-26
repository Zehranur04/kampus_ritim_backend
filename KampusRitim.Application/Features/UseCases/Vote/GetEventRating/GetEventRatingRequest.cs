using MediatR;

namespace KampusRitim.Application.Features.UseCases.Vote.GetEventRating
{
    public class GetEventRatingRequest : IRequest<GetEventRatingResponse>
    {
        public int EventId { get; set; }

        public GetEventRatingRequest(int eventId)
        {
            EventId = eventId;
        }
    }
}
