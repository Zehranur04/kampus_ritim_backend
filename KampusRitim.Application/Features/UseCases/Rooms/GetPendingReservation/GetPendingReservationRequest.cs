using MediatR;

namespace KampusRitim.Application.Features.UseCases.Rooms.GetPendingReservation
{
    public class GetPendingReservationRequest : IRequest<GetPendingReservationResponse>
    {
        // Backward-compatible: if JWT yoksa (swagger/test), email ile doğrulama yapılabilir.
        public string? AdminEmail { get; set; }
    }
}
