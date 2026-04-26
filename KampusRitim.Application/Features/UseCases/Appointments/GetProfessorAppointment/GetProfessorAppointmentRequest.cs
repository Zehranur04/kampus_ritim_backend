using MediatR;

namespace KampusRitim.Application.Features.UseCases.Appointments.GetProfessorAppointment
{
    public class GetProfessorAppointmentRequest : IRequest<GetProfessorAppointmentResponse>
    {
        public int ProfessorId { get; set; }
    }
}
