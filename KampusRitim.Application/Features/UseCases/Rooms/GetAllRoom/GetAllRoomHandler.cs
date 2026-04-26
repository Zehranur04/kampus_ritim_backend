using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Rooms.GetAllRoom
{
    public class GetAllRoomHandler : IRequestHandler<GetAllRoomRequest, GetAllRoomResponse>
    {
        private readonly IRoomRepository _roomRepository;

        public GetAllRoomHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<GetAllRoomResponse> Handle(GetAllRoomRequest request, CancellationToken cancellationToken)
        {
            // 1. Repo'dan verileri çek
            var rooms = await _roomRepository.GetAllRoomsAsync();

            // 2. Response nesnesini hazırla
            var response = new GetAllRoomResponse();

            // 3. Mapping (Entity -> DTO)
            response.Rooms = rooms.Select(room => new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Location = room.Location,
                HasProjector = room.HasProjector
            }).ToList();

            response.Success = true;
            response.Message = $"{rooms.Count} adet oda listelendi.";

            return response;
        }
    }
}
