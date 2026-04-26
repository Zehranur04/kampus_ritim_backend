using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Appointments.CreateAppointment
{
    public class CreateAppointmentHandler : IRequestHandler<CreateAppointmentRequest, CreateAppointmentResponse>
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public CreateAppointmentHandler(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<CreateAppointmentResponse> Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateAppointmentResponse();

            // Validate incoming data early to avoid DB constraint errors
            if (request.UserId <= 0)
            {
                throw new ArgumentException("UserId is required and must be a positive integer.");
            }

            if (request.Date == default)
            {
                throw new ArgumentException("Date is required and must be a valid date/time.");
            }

            // 1. Çakışma Kontrolü (Repository'deki metodumuz)
            bool isAvailable = await _appointmentRepository.IsSlotAvailableAsync(request.ProfessorId, request.Date);

            if (!isAvailable)
            {
                throw new Exception("Seçilen saatte hocanın başka bir randevusu mevcut. Lütfen başka bir saat seçiniz.");
            }

            // 2. Mapping & Kayıt
            var appointment = new Appointment
            {
                ProfessorId = request.ProfessorId,
                UserId = request.UserId,
                Date = request.Date
            };

            await _appointmentRepository.AddAsync(appointment);

            // 3. Response Hazırlama
            response.Success = true;
            response.Message = "Randevu başarıyla oluşturuldu.";
            response.AppointmentId = appointment.Id;

            return response;
        }
    }
}
