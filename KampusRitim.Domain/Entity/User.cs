using KampusRitim.Domain.Enums;

namespace KampusRitim.Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;  // Şifrenin HASH'lenmiş hali
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public ClassLevel? ClassLevel { get; set; } // Enum'u kullanıyoruz

        public UserSystemRole Role { get; set; } = UserSystemRole.Student;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UserClub> ClubMembership { get; set; } = null!;

        public ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();         // Bir kullanıcının katıldığı tüm etkinlik kayıtları

        public Profile Profile { get; set; } = null!;          // User'dan Profile'a gitmek için
    }
}

