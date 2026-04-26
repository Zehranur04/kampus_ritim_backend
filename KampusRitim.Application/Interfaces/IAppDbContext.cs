using KampusRitim.Domain.Entities; // Club entity'sinin olduğu yer
using KampusRitim.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Application.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Club> Clubs { get; }
        DbSet<Category> Categories { get; } // Kategori için de lazım

        DbSet<Event> Events { get; }
    }
}