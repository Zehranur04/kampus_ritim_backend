using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Appointments.GetMyAppointments
{
    public class GetMyAppointmentsHandler : IRequestHandler<GetMyAppointmentsRequest, GetMyAppointmentsResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyAppointmentsHandler(IAppointmentRepository appointmentRepository, ICurrentUserService currentUserService)
        {
            _appointmentRepository = appointmentRepository;
            _currentUserService = currentUserService;
        }

        public async Task<GetMyAppointmentsResponse> Handle(GetMyAppointmentsRequest request, CancellationToken cancellationToken)
        {
            var response = new GetMyAppointmentsResponse();

            var userId = _currentUserService.UserId;
            if (userId == null)
            {
                response.Success = false;
                response.Message = "User is not authenticated";
                return response;
            }

            var appointments = await _appointmentRepository.GetByUserIdAsync(userId.Value);

            response.Appointments = appointments.Select(x => new AppointmentDto
            {
                Id = x.Id,
                ProfessorName = x.Professor != null ? x.Professor.Name : string.Empty,
                UserName = x.User != null ? x.User.Name : string.Empty,
                Date = x.Date
            }).ToList();

            response.Success = true;
            return response;
        }
    }
}
