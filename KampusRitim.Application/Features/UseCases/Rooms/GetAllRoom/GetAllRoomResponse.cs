using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.Features.UseCases.Rooms.GetAllRoom
{
    public class GetAllRoomResponse
    {
        public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
