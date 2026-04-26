using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Application.Interfaces;
using KampusRitim.Domain.Entity;
using KampusRitim.Domain.Enums;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Rooms.CreateRoomReservation
{
    public class CreateRoomReservationCommandHandler : IRequestHandler<CreateRoomReservationRequest, CreateRoomReservationResponse>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateRoomReservationCommandHandler(IRoomRepository roomRepository, ICurrentUserService currentUserService)
        {
            _roomRepository = roomRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreateRoomReservationResponse> Handle(CreateRoomReservationRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateRoomReservationResponse();

            var resolvedUserId = _currentUserService.UserId;
            if (!resolvedUserId.HasValue && request.UserId > 0)
                resolvedUserId = request.UserId;

            if (!resolvedUserId.HasValue)
            {
                response.Success = false;
                response.Message = "Rezervasyon için kullanıcı girişi gerekli.";
                return response;
            }

            // 1. Müsaitlik Kontrolü
            bool isAvailable = await _roomRepository.IsRoomAvailableAsync(request.RoomId, request.StartTime, request.EndTime);

            if (!isAvailable)
            {
                response.Success = false;
                response.Message = "Seçilen saatte bu oda maalesef dolu.";
                return response;
            }

            // 2. Rezervasyon Oluşturma
            var reservation = new Reservation
            {
                RoomId = request.RoomId,
                // EventId eklemek istersen buraya ekle (Entity'de varsa)
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                Status = ReservationStatus.Pending, // Onay Bekliyor
                UserId = resolvedUserId.Value
            };

            await _roomRepository.AddReservationAsync(reservation);

            response.Success = true;
            response.ReservationId = reservation.Id;
            response.Message = "Rezervasyon talebiniz alındı.";

            return response;
        }
    }

}
