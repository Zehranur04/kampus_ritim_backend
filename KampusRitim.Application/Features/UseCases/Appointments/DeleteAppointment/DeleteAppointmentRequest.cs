using MediatR;

namespace KampusRitim.Application.Features.UseCases.Appointments.DeleteAppointment
{
    public class DeleteAppointmentRequest: IRequest<DeleteAppointmentResponse>
    {
        public int AppointmentId { get; set; }
    }
}
