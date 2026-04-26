using MediatR;

namespace KampusRitim.Application.Features.UseCases.Rooms.ApproveReservation
{

    public class ApproveReservationRequest : IRequest<ApproveReservationResponse>
    {
        public int ReservationId { get; set; }
        public string AdminEmail { get; set; } = string.Empty;
    }

}
