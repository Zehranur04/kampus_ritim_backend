using MediatR;

namespace KampusRitim.Application.UseCase.Club.CreateClub
{
    // Bu request, işlem sonucunda geriye "CreateClubResponse" dönecek.
    public class CreateClubRequest : IRequest<CreateClubResponse>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }

        public int CategoryId { get; set; } // YENİ EKLENDİ
    }
}