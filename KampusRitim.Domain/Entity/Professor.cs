namespace KampusRitim.Domain.Entity
{
    public class Professor
    {
        public int Id { get; set; } // BaseEntity olmadığı için elle ekledik
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;

        // Hangi tarihte sisteme eklendiğini görmek istersen:
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Bir hocanın birden çok randevusu olabilir
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
