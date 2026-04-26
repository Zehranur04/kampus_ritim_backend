using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Enums;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Rooms.GetPendingReservation
{
    public class GetPendingReservationHandler : IRequestHandler<GetPendingReservationRequest, GetPendingReservationResponse>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public GetPendingReservationHandler(IRoomRepository roomRepository, IUserRepository userRepository, ICurrentUserService currentUser)
        {
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<GetPendingReservationResponse> Handle(GetPendingReservationRequest request, CancellationToken cancellationToken)
        {
            var response = new GetPendingReservationResponse();

            var user = _currentUser.UserId.HasValue
                ? await _userRepository.GetByIdAsync(_currentUser.UserId.Value)
                : (!string.IsNullOrWhiteSpace(request.AdminEmail) ? await _userRepository.GetByEmailAsync(request.AdminEmail) : null);

            if (user == null)
            {
                response.Success = false;
                response.Message = "Yetkisiz işlem. Lütfen giriş yapın.";
                return response;
            }

            if (user.Role != UserSystemRole.SystemManager)
            {
                response.Success = false;
                response.Message = "Yetkisiz işlem! Sadece yönetici görüntüleyebilir.";
                return response;
            }

            var pendingList = await _roomRepository.GetPendingReservationsAsync();

            response.Success = true;
            response.Reservations = pendingList.Select(r => new PendingReservationDto
            {
                Id = r.Id,
                RoomName = r.Room != null ? r.Room.Name : "Bilinmeyen Oda",
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = "Onay Bekliyor"
            }).ToList();

            return response;
        }
    }
}
