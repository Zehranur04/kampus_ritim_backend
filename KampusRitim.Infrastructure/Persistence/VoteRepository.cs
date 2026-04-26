using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entity;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace KampusRitim.Infrastructure.Persistence
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _context;

        public VoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Vote> AddAsync(Vote vote)
        {
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
            return vote;
        }

        public async Task<bool> HasUserVotedAsync(int userId, int eventId)
        {
            return await _context.Votes
                .AnyAsync(v => v.UserId == userId && v.EventId == eventId);
        }

        public async Task<IEnumerable<Vote>> GetVotesByEventIdAsync(int eventId)
        {
            return await _context.Votes
                .Where(v => v.EventId == eventId)
                .ToListAsync();
        }
    }
}
