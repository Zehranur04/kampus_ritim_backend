using KampusRitim.Application.Interfaces;
using KampusRitim.Domain.Entities;
using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KampusRitim.Infrastructure.Context
{
    public class AppDbContext : DbContext , IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClub> UserClubs { get; set; }
        public DbSet <Event> Events { get; set; }
        public DbSet<UserEvent> UserEvents { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EF Core'a diyoruz ki:
            // Git, bu projenin (Infrastructure) içindeki tüm dosyaları (Assembly) tara,
            // 'IEntityTypeConfiguration' arayüzünü uygulayan tüm sınıfları bul ve onların 'Configure' metotlarını benim yerime otomatik olarak çalıştır.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}