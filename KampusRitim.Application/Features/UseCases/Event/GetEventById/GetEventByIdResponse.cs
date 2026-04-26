using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.Event.GetEventById
{
    public class GetEventByIdResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public EventDto? Event { get; set; }             // Etkinlik bulunamazsa null olabilir
    }
}