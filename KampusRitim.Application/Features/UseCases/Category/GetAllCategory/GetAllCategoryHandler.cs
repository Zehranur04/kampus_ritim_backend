using KampusRitim.Application.Features.UseCases.Category.GetAllCategory;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.Features.UseCases.Categories.GetAllCategories
{
    public class GetAllCategoryHandler : IRequestHandler<GetAllCategoryRequest, List<GetAllCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<GetAllCategoryResponse>> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
        {
            // Predicate olarak x => true veriyoruz, yani "hepsini getir"
            var categories = await _categoryRepository.GetListAsync(x => true);

            // Entity -> DTO Dönüşümü
            return categories.Select(c => new GetAllCategoryResponse
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}