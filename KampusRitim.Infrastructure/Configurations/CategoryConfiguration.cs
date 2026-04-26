using KampusRitim.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            // Primary Key
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            // Name alanı zorunlu ve benzersiz
            builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);

            // Aynı isimde iki kategori olamaz
            builder.HasIndex(x => x.Name)
                .IsUnique();

            // --- İlişki Ayarları ---
            // Bir kategorinin birden fazla Kulübü olabilir
            builder.HasMany(c => c.Clubs)
                .WithOne(club => club.Category)
                .HasForeignKey(club => club.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategori silinirse, o kategorideki kulüpler silinmesin

            // Bir kategorinin birden fazla Etkinliği olabilir
            builder.HasMany(c => c.Events)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategori silinirse, o kategorideki etkinlikler silinmesin
        }
    }
}

