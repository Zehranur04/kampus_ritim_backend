using MediatR;

namespace KampusRitim.Application.Features.UseCases.Category.GetAllCategory
{
    public class GetAllCategoryRequest : IRequest<List<GetAllCategoryResponse>>
    {
    }
}
