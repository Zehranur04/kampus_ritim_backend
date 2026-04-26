using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Appointments.GetProfessorAppointment
{
    public class GetProfessorAppointmentValidation : AbstractValidator<GetProfessorAppointmentRequest>
    {
        public GetProfessorAppointmentValidation()
        {
            RuleFor(x => x.ProfessorId)
                .GreaterThan(0).WithMessage("Geçersiz Hoca ID.");
        }
    }
   
}
