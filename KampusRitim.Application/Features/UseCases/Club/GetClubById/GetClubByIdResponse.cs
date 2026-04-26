using KampusRitim.Application.Features.Dtos; // ClubDto'yu tanımaıs için gerekli

namespace KampusRitim.Application.UseCases.Club.GetClubById
{
    public class GetClubByIdResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public ClubDto? Club { get; set; }
    }
}
