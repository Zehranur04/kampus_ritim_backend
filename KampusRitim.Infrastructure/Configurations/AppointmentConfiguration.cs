using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(x => x.Id);

            // Professor İlişkisi
            builder.HasOne(a => a.Professor)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);

            // User (Öğrenci) İlişkisi - GÜNCELLENDİ
            builder.HasOne(a => a.User)
                .WithMany() // User içinde 'Appointments' listesi yoksa boş bırak
                .HasForeignKey(a => a.UserId) // StudentId yerine UserId oldu
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse randevusu da silinsin
        }
    }
}
