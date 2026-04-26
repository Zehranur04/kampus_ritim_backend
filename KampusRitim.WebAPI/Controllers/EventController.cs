using KampusRitim.Application.UseCases.Event.CreateEvent;
using KampusRitim.Application.UseCases.Event.DeleteEvent;
using KampusRitim.Application.UseCases.Event.GetAllEvent;
using KampusRitim.Application.UseCases.Event.GetEventById;
using KampusRitim.Application.UseCases.Event.UpdateEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        // Artık Service yok, Mediator var!
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. LİSTELEME (GET api/events)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Query nesnesi oluşturup gönderiyoruz
            var response = await _mediator.Send(new GetAllEventRequest());
            return Ok(response);
        }

        // 2. DETAY GETİRME (GET api/events/5)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetEventByIdRequest { Id = id });

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }

        // 3. OLUŞTURMA (POST api/events)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // 4. GÜNCELLEME (PUT api/events/5)
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventRequest request)
        {
            // URL'den gelen ID'yi Request nesnesine işliyoruz ki karışıklık olmasın
            request.Id = id;

            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response); // veya NotFound duruma göre

            return Ok(response);
        }

        // 5. SİLME (DELETE api/events/5)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteEventRequest { Id = id });

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }
    }
}