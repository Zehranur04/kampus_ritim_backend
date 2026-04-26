using KampusRitim.Application.Features.Dtos; // DTO'yu buradan görüyor

namespace KampusRitim.Application.UseCases.Event.GetAllEvent
{
    public class GetAllEventResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public IEnumerable<EventDto> Events { get; set; } = new List<EventDto>();
    }
}