using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampusRitim.Application.Features.UseCases.Rooms.CreateRoomReservation
{
    public class CreateRoomReservationResponse
    {
        public int ReservationId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
