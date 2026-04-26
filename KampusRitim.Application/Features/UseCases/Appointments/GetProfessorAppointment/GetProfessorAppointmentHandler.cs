using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Appointments.GetProfessorAppointment
{
    public class GetProfessorAppointmentHandler : IRequestHandler<GetProfessorAppointmentRequest, GetProfessorAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public GetProfessorAppointmentHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<GetProfessorAppointmentResponse> Handle(GetProfessorAppointmentRequest request, CancellationToken cancellationToken)
        {
            // 1. Repo'dan veriyi çek
            var appointments = await _appointmentRepository.GetByProfessorIdAsync(request.ProfessorId);

            // 2. Response nesnesini hazırla
            var response = new GetProfessorAppointmentResponse();

            // 3. Mapping (Entity -> DTO)
            response.Appointments = appointments.Select(x => new AppointmentDto
            {
                Id = x.Id,
                ProfessorName = x.Professor != null ? x.Professor.Name : "",
                Date = x.Date
                // User bilgisi gerekirse buraya eklenebilir
            }).ToList();

            response.Success = true;
            return response;
        }
    }
}
