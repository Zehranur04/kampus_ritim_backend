using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profiles");

            // Id kolonunu PK anahtarı olarak belirle
            builder.HasKey(p => p.Id);


            builder.Property(p => p.Bio)
                .IsRequired();

            // ProfileImageUrl alanı null olabilir 
            builder.Property(p => p.ProfileImageUrl)
                .IsRequired(false);

            // CreatedAt alanı için veritabanı seviyesinde bir varsayılan değer atar.
            builder.Property(p => p.CreatedAt)
                  .HasDefaultValueSql("NOW()");

            builder.Property(p => p.ClassLevel)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired(false);

            // İlişki Ayarı (User silinirse Profil de silinsin)
            builder.HasOne(p => p.User)
                   .WithOne(u => u.Profile)
                   .HasForeignKey<Profile>(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}