namespace KampusRitim.Domain.Entity
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Title { get; set; } // Örn: "Prof. Dr.", "Senior Developer"
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

