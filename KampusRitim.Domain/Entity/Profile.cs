using KampusRitim.Domain.Enums;

namespace KampusRitim.Domain.Entity 
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public ClassLevel? ClassLevel { get; set; }

        public string Bio { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }   // Foreign Key
        public User User { get; set; } = null!;  // Bu, EF Core'a Profile'ın bir User'a ait olduğunu söyler

    }
}
