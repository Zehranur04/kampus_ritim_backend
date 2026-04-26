using KampusRitim.Domain.Entities;
using System.Linq.Expressions;

namespace KampusRitim.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        // Kategori ID'lerine göre filtreleyip liste getiren metot
        Task<List<Category>> GetListAsync(Expression<Func<Category, bool>> predicate);

    }
}