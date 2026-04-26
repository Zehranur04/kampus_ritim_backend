using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Department).HasMaxLength(100);

            // SEED DATA - DÜZELTİLMİŞ HALİ
            // DateTime.UtcNow yerine sabit bir tarih (Örn: 2025-01-01) veriyoruz.
            var fixedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new Professor
                {
                    Id = 1,
                    Name = "Dr. Hatice Yurtseven Yılmaz",
                    Department = "Türkçe Öğretmenliği",
                    CreatedAt = fixedDate // ARTIK SABİT
                },
                new Professor
                {
                    Id = 2,
                    Name = "Prof. Dr. Ahmet Yılmaz",
                    Department = "Bilgisayar Mühendisliği",
                    CreatedAt = fixedDate // ARTIK SABİT
                },
                new Professor
                {
                    Id = 3,
                    Name = "Doç. Dr. Ayşe Demir",
                    Department = "Yazılım Mühendisliği",
                    CreatedAt = fixedDate // ARTIK SABİT
                }
            );
        }
    }
}
