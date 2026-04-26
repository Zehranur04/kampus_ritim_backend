using MediatR;

namespace KampusRitim.Application.Features.UseCases.UserClub.UpdateClub
{
    public class UpdateClubRequest : IRequest<UpdateClubResponse>
    {
        public int Id { get; set; } // Hangi kulüp güncellenecek?
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}