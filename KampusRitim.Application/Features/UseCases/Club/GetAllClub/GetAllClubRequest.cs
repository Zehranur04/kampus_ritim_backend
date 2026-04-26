using MediatR;

namespace KampusRitim.Application.UseCase.Club.GetAllClub
{
    public class GetAllClubRequest : IRequest<List<GetAllClubResponse>>
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
    }
}
