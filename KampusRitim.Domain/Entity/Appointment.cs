namespace KampusRitim.Domain.Entity
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }


        // 1. İlişki: Hoca (Professor)
        public int ProfessorId { get; set; }
        public Professor Professor { get; set; } = null!;


        // 2. İlişki: Randevuyu alan Kullanıcı (User). Artık Student değil, direkt User diyoruz.
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
