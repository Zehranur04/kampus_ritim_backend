using MediatR;

namespace KampusRitim.Application.UseCases.Club.DeleteClub
{
    public class DeleteClubRequest : IRequest<DeleteClubResponse>
    {
        public int Id { get; set; }
    }
}