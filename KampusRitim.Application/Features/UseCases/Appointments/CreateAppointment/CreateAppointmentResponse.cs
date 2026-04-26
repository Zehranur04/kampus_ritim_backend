namespace KampusRitim.Application.Features.UseCases.Appointments.CreateAppointment
{
    public class CreateAppointmentResponse
    {
        public int AppointmentId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
