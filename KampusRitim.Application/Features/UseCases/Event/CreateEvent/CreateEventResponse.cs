namespace KampusRitim.Application.UseCases.Event.CreateEvent
{
    public class CreateEventResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int EventId { get; set; } // Oluşan etkinliğin numarası
    }
}