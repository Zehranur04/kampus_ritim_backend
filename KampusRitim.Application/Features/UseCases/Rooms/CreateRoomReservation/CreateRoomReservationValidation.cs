using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Rooms.CreateRoomReservation
{
    public class CreateRoomReservationValidation : AbstractValidator<CreateRoomReservationRequest>
    {
        public CreateRoomReservationValidation()
        {
            RuleFor(x => x.RoomId).GreaterThan(0).WithMessage("Oda seçimi zorunludur.");
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Kullanıcı bilgisi zorunludur.");
            RuleFor(x => x.StartTime).GreaterThan(DateTime.Now).WithMessage("Geçmişe rezervasyon yapılamaz.");
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage("Bitiş saati başlangıçtan sonra olmalıdır.");
        }
    }
}
