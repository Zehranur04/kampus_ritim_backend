using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Event.DeleteEvent
{
    public class DeleteEventHandler : IRequestHandler<DeleteEventRequest, DeleteEventResponse>
    {
        private readonly IEventRepository _eventRepo;

        public DeleteEventHandler(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        public async Task<DeleteEventResponse> Handle(DeleteEventRequest request, CancellationToken cancellationToken)
        {
            // 1. ADIM: Önce silinecek etkinliği bulalım.
            // (Bu işlem için konuşmacı detayına ihtiyacımız yok, düz GetByIdAsync yeterli)
            var eventToDelete = await _eventRepo.GetByIdAsync(request.Id);

            // 2. ADIM: Güvenlik Kontrolü - Etkinlik var mı?
            if (eventToDelete == null)
            {
                // OLUMSUZ SENARYO: Etkinlik yoksa Repository'e gitmeye gerek yok.
                return new DeleteEventResponse
                {
                    IsSuccess = false,
                    Message = $"Silinmek istenen {request.Id} numaralı etkinlik bulunamadı."
                };
            }

            // 3. ADIM: Varsa silme işlemini gerçekleştir.
            // Repository'ye bulduğumuz entity'yi teslim ediyoruz.
            await _eventRepo.DeleteAsync(eventToDelete);

            // OLUMLU SENARYO
            return new DeleteEventResponse
            {
                IsSuccess = true,
                Message = "Etkinlik başarıyla silindi."
            };
        }
    }
}