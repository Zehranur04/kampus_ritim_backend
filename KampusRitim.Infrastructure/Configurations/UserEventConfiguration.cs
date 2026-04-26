using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Persistence.Configurations
{
    public class UserEventConfiguration : IEntityTypeConfiguration<UserEvent>
    {
        public void Configure(EntityTypeBuilder<UserEvent> builder)
        {
            // 1. Tablo Adı
            builder.ToTable("UserEvents");

            // 2. Composite Primary Key (En Önemli Kısım)
            // Bu tabloda tek bir "Id" yok. UserId ve EventId ikilisi benzersizdir.
            builder.HasKey(ue => new { ue.UserId, ue.EventId });

            // 3. JoinedAt Ayarı
            builder.Property(ue => ue.JoinedAt)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            // 4. İlişkiler (Relationships)

            // User ile İlişki
            builder.HasOne(ue => ue.User)
                .WithMany(u => u.UserEvents) // User entity'sinde bu koleksiyon olmalı
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse katılımı da silinsin

            // Event ile İlişki
            builder.HasOne(ue => ue.Event)
                .WithMany(e => e.UserEvents) // Event entity'sinde bu koleksiyon olmalı
                .HasForeignKey(ue => ue.EventId)
                .OnDelete(DeleteBehavior.Cascade); // Etkinlik silinirse katılımcı listesi de silinsin
        }
    }
}