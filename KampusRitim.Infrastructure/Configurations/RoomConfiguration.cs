using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KampusRitim.Infrastructure.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Location).HasMaxLength(200);

            // SEED DATA: Okuldaki Odalar
            builder.HasData(
                new Room { Id = 1, Name = "Ana Konferans Salonu", Capacity = 300, Location = "Rektörlük Binası", HasProjector = true },
                new Room { Id = 2, Name = "Seminer Odası B", Capacity = 50, Location = "Mühendislik Fakültesi 1. Kat", HasProjector = true },
                new Room { Id = 3, Name = "Çalışma Salonu (Sessiz)", Capacity = 30, Location = "Kütüphane", HasProjector = false }
            );
        }
    }
}
