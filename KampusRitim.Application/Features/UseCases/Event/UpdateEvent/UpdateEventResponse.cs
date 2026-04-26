namespace KampusRitim.Application.UseCases.Event.UpdateEvent
{
    public class UpdateEventResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int? EventId { get; set; }
    }
}
