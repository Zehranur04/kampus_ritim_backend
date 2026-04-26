using MediatR;

namespace KampusRitim.Application.Features.UseCases.Rooms.CreateRoomReservation
{

    public class CreateRoomReservationRequest : IRequest<CreateRoomReservationResponse>
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

}
