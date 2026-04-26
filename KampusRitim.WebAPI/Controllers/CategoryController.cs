using KampusRitim.Application.Features.UseCases.Category.GetAllCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc; // <-- İŞTE BU SATIR CONTROLLERBASE'İ TANITIR

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase 
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllCategoryRequest());
            return Ok(response);
        }
    }
}