using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.Features.UseCases.GetMyClub
{
    public class GetMyClubResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public List<MyClubDto> Clubs { get; set; } = new();
    }
}
