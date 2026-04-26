using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Rooms.ApproveReservation
{
    public class ApproveReservationValidation : AbstractValidator<ApproveReservationRequest>
    {
        public ApproveReservationValidation()
        {
            RuleFor(x => x.ReservationId).GreaterThan(0).WithMessage("Geçersiz Rezervasyon ID.");
            RuleFor(x => x.AdminEmail).NotEmpty().EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
        }
    }

}
