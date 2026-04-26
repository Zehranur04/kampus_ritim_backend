using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Domain.Entities;
using KampusRitim.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KampusRitim.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetListAsync(Expression<Func<Category, bool>> predicate)
        {
            return await _context.Categories
                .Where(predicate)
                .ToListAsync();
        }
    }
}
