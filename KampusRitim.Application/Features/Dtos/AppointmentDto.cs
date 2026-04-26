namespace KampusRitim.Application.Features.Dtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string ProfessorName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
