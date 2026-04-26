using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IVoteRepository
    {
        Task<Vote> AddAsync(Vote vote);

        // Bir kullanıcı bir etkinliğe daha önce oy vermiş mi?
        Task<bool> HasUserVotedAsync(int userId, int eventId);

        // Bir etkinliğin oylarını getir (Ortalama hesaplamak için)
        // (Sadece okunabilir liste veya IQueryable dönebiliriz, şimdilik List dönelim)
        Task<IEnumerable<Vote>> GetVotesByEventIdAsync(int eventId);

    }
}
