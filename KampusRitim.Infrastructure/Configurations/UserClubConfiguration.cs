using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class UserClubConfiguration : IEntityTypeConfiguration<UserClub>
    {
        public void Configure(EntityTypeBuilder<UserClub> builder)
        {
            builder.ToTable("User Clubs");
         
            // Bir kullanıcı bir kulübe sadece 1 kez üye olabilir.
            // (UserId, ClubId) ikilisi birlikte anahtardır.
            builder.HasKey(uc => new { uc.UserId, uc.ClubId });

            // 'User' ile olan ilişki
            builder.HasOne(uc => uc.User) // UserClubs'ın 1 User'ı vardır
                .WithMany(u => u.ClubMembership) // 1 User'ın çok 'KulupUyelikleri' vardır
                .HasForeignKey(uc => uc.UserId); // FK 'UserId'dir

            // 'Club' ile olan ilişki
            builder.HasOne(uc => uc.Club) // UserClubs'ın 1 Club'ı vardır
                .WithMany(c => c.Member) // 1 Club'ın çok 'Uyeler'i vardır
                .HasForeignKey(uc => uc.ClubId); // FK 'ClubId'dir


            // 'JoinDate' alanı (Required / NOT NULL)
            builder.Property(uc => uc.JoinDate)
                .IsRequired();

            // 'ClubRole' alanı (Required / nvarchar(20))
            builder.Property(uc => uc.ClubRole)
                .IsRequired()
                .HasConversion<string>() // Enum'u veritabanında 'Member', 'Admin' string olarak sakla
                .HasMaxLength(20);
        }
    }
}