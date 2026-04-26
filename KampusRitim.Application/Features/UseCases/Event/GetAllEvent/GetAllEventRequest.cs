using KampusRitim.Application.Features.Dtos; // EventListDto'nun olduğu yer
using MediatR;

namespace KampusRitim.Application.UseCases.Event.GetAllEvent
{
    public class GetAllEventRequest : IRequest<GetAllEventResponse>
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
    }
}