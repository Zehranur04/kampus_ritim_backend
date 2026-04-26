namespace KampusRitim.Application.Features.Dtos
{
    //Bir kulübe tıklandığında, içindeki üyeleri ve rollerini listelemek için
    public record ClubMemberDto(
        int UserId,
        string UserName,
        string UserEmail,
        string ClubRole, // "Admin" - "Member"
        DateTime JoinDate
    );
}
