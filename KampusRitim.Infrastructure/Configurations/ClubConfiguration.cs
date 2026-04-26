using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.ToTable("Clubs");

            // Id (PK, IDENTITY(1,1))
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            // "Boş olamaz" ve "100 karakterden uzun olamaz" der.
            builder.Property(c => c.Name)
                .IsRequired().HasMaxLength(100);

            //"Aynı isimde iki farklı kulüp olamaz" kuralını ekler.
            builder.HasIndex(c => c.Name) 
                .IsUnique(); 

            // 'string?' zaten 'nullable' ve 'nvarchar(max)' varsayılan. O yüzden ek bir kurala gerek yok.
            builder.Property(c => c.Description);

            builder.Property(c => c.ProfileImageUrl)
                .HasMaxLength(255);

            builder.Property(c => c.CreatedAt)
                .IsRequired();


            // İLİŞKİ AYARI (Relation):
            builder.HasOne(c => c.Category)      // Bir Kulübün BİR Kategorisi vardır.
                   .WithMany(cat => cat.Clubs)   // Bir Kategorinin ÇOK Kulübü olabilir.
                   .HasForeignKey(c => c.CategoryId) // Bağlantı anahtarı CategoryId'dir.
                   .OnDelete(DeleteBehavior.Restrict); // Kategori silinirse kulüpler silinmesin (Güvenlik önlemi)

            //builder.HasMany(c => c.BoardMembers)
            //       .WithOne(bm => bm.Club)         // Karşı tarafta (ClubBoardMember) 'Club' nesnesi var
            //       .HasForeignKey(bm => bm.ClubId) // Karşı tarafta 'ClubId' FK var
            //       .OnDelete(DeleteBehavior.Cascade); // Kulüp silinirse, yönetim listesi de silinsin!
        }
    }
}

