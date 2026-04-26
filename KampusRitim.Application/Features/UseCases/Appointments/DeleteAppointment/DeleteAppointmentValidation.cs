using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Appointments.DeleteAppointment
{ 
public class DeleteAppointmentValidation : AbstractValidator<DeleteAppointmentRequest>
{
    public DeleteAppointmentValidation()
    {
        RuleFor(x => x.AppointmentId)
            .GreaterThan(0).WithMessage("Geçersiz randevu ID.");
    }
}
}
