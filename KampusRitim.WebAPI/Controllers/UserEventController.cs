using KampusRitim.Application.UseCases.UserEvent.JoinEvent;
using KampusRitim.Application.UseCases.UserEvent.LeaveEvent;
using KampusRitim.Application.UseCases.UserEvent.GetMyEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/userevent")] // veya "api/user-events" (Standartlara göre tireli kullanımı yaygındır)
    [ApiController]
    [Authorize] // Bu controller'a sadece giriş yapmış kullanıcılar erişebilir
    public class UserEventController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserEventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ---------------------------------------------------------
        // MINE (Katıldığım Etkinlikler)
        // ---------------------------------------------------------
        // GET: api/userevent/mine
        [HttpGet("mine")]
        public async Task<IActionResult> Mine()
        {
            var response = await _mediator.Send(new GetMyEventsRequest());

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // ---------------------------------------------------------
        // JOIN (Etkinliğe Katıl)
        // ---------------------------------------------------------
        // POST: api/userevent/join
        // Body: { "eventId": 5 }
        [HttpPost("join")]
        public async Task<IActionResult> Join([FromBody] JoinEventRequest request)
        {
            // Kullanıcı ID'sini çözmekle uğraşmıyoruz, Handler hallediyor.
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
            {
                // Hata mesajını (Kontenjan dolu, tarih geçti vs.) dönüyoruz
                return BadRequest(response);
            }

            return Ok(response);
        }

        // ---------------------------------------------------------
        // LEAVE (Etkinlikten Ayrıl)
        // ---------------------------------------------------------
        // DELETE: api/userevent/leave/5
        [HttpDelete("leave/{eventId}")]
        public async Task<IActionResult> Leave(int eventId)
        {
            // URL'den gelen ID'yi Command nesnesine çeviriyoruz
            var request = new LeaveEventRequest { EventId = eventId };

            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
            {
                return BadRequest(response); // Veya NotFound(response)
            }

            return Ok(response);
        }
    }
}