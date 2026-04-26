using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.Features.UseCases.Appointments.GetMyAppointments
{
    public class GetMyAppointmentsResponse
    {
        public List<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
