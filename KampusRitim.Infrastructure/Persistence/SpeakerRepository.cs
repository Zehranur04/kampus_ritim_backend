using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;


namespace KampusRitim.Infrastructure.Repositories
{
    public class SpeakerRepository : ISpeakerRepository
    {
        private readonly AppDbContext _context;

        public SpeakerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Speaker?> GetByIdAsync(int id)
        {
            return await _context.Speakers.FindAsync(id);
        }

        public async Task<Speaker> AddAsync(Speaker speaker)
        {
            await _context.Speakers.AddAsync(speaker);
            await _context.SaveChangesAsync();
            return speaker;
        }
    }
}