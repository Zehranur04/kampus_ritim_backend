using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.Features.UseCases.Rooms.GetPendingReservation
{
    public class GetPendingReservationResponse
    {
        public List<PendingReservationDto> Reservations { get; set; } = new();
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
