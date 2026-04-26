using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface ISpeakerRepository
    {
        Task<Speaker?> GetByIdAsync(int id);

        Task<Speaker> AddAsync(Speaker speaker);
    }
}