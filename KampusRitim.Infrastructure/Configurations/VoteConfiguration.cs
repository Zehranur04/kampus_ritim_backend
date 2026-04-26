using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class VoteConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            builder.ToTable("Votes");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Score)
                .IsRequired();

            // ÖNEMLİ KURAL: Bir User, aynı Event'e ikinci kez oy veremez.
            // (UserId + EventId) ikilisi benzersiz (Unique) olmalı.
            builder.HasIndex(v => new { v.UserId, v.EventId })
                .IsUnique();


            builder.HasOne(v => v.User)
                .WithMany() // User entity'sine 'Votes' koleksiyonu eklemediysek WithMany boş kalabilir veya User.cs güncellenebilir.
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse oyları da silinsin (tercihe bağlı)

            builder.HasOne(v => v.Event)
                .WithMany()
                .HasForeignKey(v => v.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
