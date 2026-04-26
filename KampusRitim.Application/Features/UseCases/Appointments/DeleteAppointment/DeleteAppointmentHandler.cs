using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Appointments.DeleteAppointment
{
    public class DeleteAppointmentHandler : IRequestHandler<DeleteAppointmentRequest, DeleteAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public DeleteAppointmentHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<DeleteAppointmentResponse> Handle(DeleteAppointmentRequest request, CancellationToken cancellationToken)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);

            if (appointment == null)
            {
                throw new Exception("Silinmek istenen randevu bulunamadı.");
            }

            await _appointmentRepository.DeleteAsync(appointment);

            return new DeleteAppointmentResponse
            {
                Success = true,
                Message = "Randevu başarıyla iptal edildi."
            };
        }
    }
}
