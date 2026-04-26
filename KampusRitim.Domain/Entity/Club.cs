using KampusRitim.Domain.Entities;

namespace KampusRitim.Domain.Entity
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserClub> Member { get; set; } = null!;     //  'UserClubs' (ara tablo) ile ilişkiyi kurar.
        public ICollection<Event> Events { get; set; } = new List<Event>();

        // public ICollection<ClubBoardMember> BoardMembers { get; set; } = new List<ClubBoardMember>();
        public int CategoryId { get; set; } // Foreign Key (Yabancı Anahtar)
        public Category Category { get; set; } = null!;


    }
}