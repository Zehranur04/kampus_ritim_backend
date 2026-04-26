using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");

            builder.HasKey(e => e.Id);

            // Title alanı zorunludur (IsRequired) ve maksimum 100 karakterdir.
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            // Description alanı zorunludur ve maksimum 1000 karakterdir.
            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(1000);

            // Location alanı zorunludur ve maksimum 200 karakterdir.
            builder.Property(e => e.Location)
                .IsRequired()
                .HasMaxLength(200);

            // Time (Zaman) alanı zorunludur.
            builder.Property(e => e.Time)
                .IsRequired();

            // Quota (Kontenjan) alanı zorunludur.
            builder.Property(e => e.Quota)
                .IsRequired();

            // CertificateDetails alanı zorunlu DEĞİLDİR (nullable).
            builder.Property(e => e.CertificateDetails)
                .IsRequired(false); // false = nullable

            // --- İlişki Ayarları ---
            // 'Event' ile 'Speaker' arasındaki ilişki (Bire Çok İlişki)
            // Bir Speaker'ın (Konuşmacı) birden çok Event'i (Etkinlik) olabilir.
            // Bir Event'in sadece bir Speaker'ı olabilir.
            builder.HasOne(e => e.Speaker)         // Event'in bir Speaker'ı var
                .WithMany()                        // Speaker'ın birden çok Event'i olabilir
                                                   // (Eğer Speaker sınıfında List<Event> yoksa burası boş kalır)
                .HasForeignKey(e => e.SpeakerId)   // Yabancı anahtar (Foreign Key) SpeakerId'dir
                .OnDelete(DeleteBehavior.Restrict); // Bir Speaker silinirse, o Speaker'a bağlı
                                                    // Event'lerin silinmesini engelle (Restrict).
                                                    // Ya da duruma göre NoAction / SetNull seçilebilir.

            builder.HasOne(e => e.Category)
               .WithMany(cat => cat.Events)
               .HasForeignKey(e => e.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Club)
                .WithMany(c => c.Events) // Club entity'sinde 'Events' listesi varsa
                .HasForeignKey(e => e.ClubId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.UserEvents)
                .WithOne(ue => ue.Event)
                .HasForeignKey(ue => ue.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Etkinlik silinirse katılım kayıtları da silinsin

        }
    }
}
