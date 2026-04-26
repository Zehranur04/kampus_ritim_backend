using KampusRitim.Domain.Entity;
using KampusRitim.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // [Key] ve IDENTITY(1,1) kuralı
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired().HasMaxLength(100);

            // Surname: Required (NOT NULL) ve MaxLength(100) kuralı
            builder.Property(u => u.Surname)
                .IsRequired().HasMaxLength(100);

            // Email: Required, MaxLength(100) VE UNIQUE kuralı
            builder.Property(u => u.Email)
                .IsRequired().HasMaxLength(100);

            builder.HasIndex(u => u.Email) // Email kolonu için bir Index (dizin) oluştur
                .IsUnique(); // ve bu index'in Benzersiz (UNIQUE) olmasını sağla.

            // PasswordHash: Required ve MaxLength(32) (varbinary(32)) kuralı
            builder.Property(u => u.PasswordHash)
                .IsRequired().HasMaxLength(32);

            // Faculty: Opsiyonel (nullable) ama MaxLength(100) kuralı
            builder.Property(u => u.Faculty)
                .HasMaxLength(100);

            // Department: Opsiyonel (nullable) ama MaxLength(100) kuralı
            builder.Property(u => u.Department)
                .HasMaxLength(100);

            // CreatedAt: Required kuralı
            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(p => p.ClassLevel)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(u => u.Role)
                .HasConversion<int>()
                .HasDefaultValue(UserSystemRole.Student)
                .IsRequired();
        }
    }
}