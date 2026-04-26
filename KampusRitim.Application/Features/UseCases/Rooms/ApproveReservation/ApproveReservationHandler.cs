using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Enums;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Rooms.ApproveReservation
{
    public class ApproveReservationCommandHandler : IRequestHandler<ApproveReservationRequest, ApproveReservationResponse>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUserService _currentUser;

        public ApproveReservationCommandHandler(IRoomRepository roomRepository, IUserRepository userRepository, ICurrentUserService currentUser)
        {
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task<ApproveReservationResponse> Handle(ApproveReservationRequest request, CancellationToken cancellationToken)
        {
            var response = new ApproveReservationResponse();

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
                response.Message = "Yetkisiz işlem! Sadece yönetici onaylayabilir.";
                return response;
            }

            // Durum Güncelleme
            await _roomRepository.UpdateReservationStatusAsync(request.ReservationId, ReservationStatus.Approved);

            response.Success = true;
            response.Message = "Rezervasyon başarıyla onaylandı.";
            return response;
        }
    }
}
