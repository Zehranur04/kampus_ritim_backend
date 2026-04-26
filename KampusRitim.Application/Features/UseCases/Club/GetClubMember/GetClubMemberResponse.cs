using KampusRitim.Application.Features.Dtos; // DTO'nun olduğu namespace

namespace KampusRitim.Application.UseCases.Club.GetClubMember
{
    public class GetClubMemberResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Üye listesi. Boş gelebilir, o yüzden nullable yapmadım ama içi boş olabilir.
        public IEnumerable<ClubMemberDto> Members { get; set; } = new List<ClubMemberDto>();
    }
}