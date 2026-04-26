using MediatR;

namespace KampusRitim.Application.Features.UseCases.Appointments.CreateAppointment
{
    public class CreateAppointmentRequest : IRequest<CreateAppointmentResponse>
    {
        public int ProfessorId { get; set; }
        public int UserId { get; set; } // Randevuyu kim alıyor?
        public DateTime Date { get; set; }
    }
}
