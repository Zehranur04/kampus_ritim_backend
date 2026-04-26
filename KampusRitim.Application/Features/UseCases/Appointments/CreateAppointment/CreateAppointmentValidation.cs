using FluentValidation;
using System;

namespace KampusRitim.Application.Features.UseCases.Appointments.CreateAppointment
{
    public class CreateAppointmentValidation : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentValidation()
        {
            RuleFor(x => x.ProfessorId)
                .GreaterThan(0).WithMessage("Hoca seçimi zorunludur.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("Kullanıcı bilgisi zorunludur.");

            RuleFor(x => x.Date)
                .GreaterThan(DateTime.Now).WithMessage("Geçmiş bir tarihe randevu alamazsınız.")
                .NotNull().WithMessage("Tarih alanı boş olamaz.");
        }
    }
}
