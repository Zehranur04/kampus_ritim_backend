using MediatR;

namespace KampusRitim.Application.UseCases.Event.DeleteEvent
{
    // İşlem sonucunda başarılı/başarısız bilgisini döneceğiz.
    public class DeleteEventRequest : IRequest<DeleteEventResponse>
    {
        public int Id { get; set; }
    }
}