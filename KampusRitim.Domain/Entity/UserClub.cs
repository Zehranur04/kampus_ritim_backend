using KampusRitim.Domain.Enums;

namespace KampusRitim.Domain.Entity
{
    public class UserClub
    {
        public int UserId { get; set; } 
        public User User { get; set; } = null!;

        public int ClubId { get; set; } 
        public Club Club { get; set; } = null!;

        public DateTime JoinDate { get; set; } = DateTime.UtcNow;

        // 'ClubRole' enum'unu (Member/Admin) ekledim
        public ClubRole ClubRole { get; set; } = ClubRole.Member;
    }
}
