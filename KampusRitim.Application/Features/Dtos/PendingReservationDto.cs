namespace KampusRitim.Application.Features.Dtos
{
    public class PendingReservationDto
    {
        public int Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}