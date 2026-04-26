using MediatR;

namespace KampusRitim.Application.UseCases.Event.GetEventById
{
    public class GetEventByIdRequest : IRequest<GetEventByIdResponse>         // Geriye GetEventByIdResponse dönecek
    {
        public int Id { get; set; }
    }
}