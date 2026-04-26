using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllAsync(string? searchTerm = null, int? categoryId = null);
        Task<IEnumerable<Event>> GetAllAsync();
        Task<Event?> GetByIdWithSpeakerAsync(int eventId); // Detay için (Include Speaker)
        Task<Event?> GetByIdAsync(int eventId);            // Update/Delete bulmak için (Sade)
        Task<Event> AddAsync(Event entity);
        Task<Event> UpdateAsync(Event entity);
        Task DeleteAsync(Event eventEntity); // Doğru: Entity alıyor

        Task<Event?> GetByIdWithParticipantsAsync(int eventId);
    }
}