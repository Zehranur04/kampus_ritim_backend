using MediatR;

namespace KampusRitim.Application.UseCases.Profile.GetProfileById
{
    public class GetProfileByIdRequest : IRequest<GetProfileByIdResponse>
    {
        public int Id { get; set; }
    }
}
