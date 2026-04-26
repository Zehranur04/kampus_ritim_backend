namespace KampusRitim.Application.UseCases.UserEvent.GetMyEvents
{
    public class GetMyEventsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<int> EventIds { get; set; } = new();
    }
}
