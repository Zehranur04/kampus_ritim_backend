using FluentValidation;

namespace KampusRitim.Application.Features.UseCases.Rooms.GetAllRoom
{
    public class GetAllRoomsQueryValidation : AbstractValidator<GetAllRoomRequest>
    {
        public GetAllRoomsQueryValidation()
        {
            // Şu an giriş parametresi olmadığı için kural yazmıyoruz.
            // İleride filtreleme eklenirse buraya kurallar gelir.
        }
    }
}
