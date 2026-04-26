using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Rooms.GetPendingReservation
{
    public class GetPendingReservationValidation : AbstractValidator<GetPendingReservationRequest>
    {
        public GetPendingReservationValidation()
        {
            // Parametre zorunlu değil; token yoksa AdminEmail format kontrolü yapıyoruz.
            RuleFor(x => x.AdminEmail)
                .EmailAddress().WithMessage("Geçerli bir mail formatı giriniz.")
                .When(x => !string.IsNullOrWhiteSpace(x.AdminEmail));
        }
    }
}
