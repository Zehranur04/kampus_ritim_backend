using KampusRitim.Domain.Enums;

namespace KampusRitim.Application.Features.Dtos
{
    public class MyClubDto
    {
        public int ClubId { get; set; }
        public string? ClubName { get; set; }
        public string? ClubDescription { get; set; }
        public string? ClubProfileImageUrl { get; set; }
        public ClubRole ClubRole { get; set; } // Üye mi, Başkan mı?
        public DateTime JoinDate { get; set; }
    }
}
