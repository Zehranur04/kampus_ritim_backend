using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedClub;
using KampusRitim.Application.Features.UseCases.Recommendation.GetRecommendedEvent;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecommendationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // -------------------------------------------------------------------------
        // KULÜP ÖNERİSİ (YENİ SİSTEM)
        // -------------------------------------------------------------------------

        // Kullanıcının yazdığı serbest metne (Prompt) göre kulüp önerir.
        // POST: api/recommendations/clubs
        [HttpPost("clubs")]
        public async Task<IActionResult> GetRecommendedClubs([FromBody] GetRecommendedClubRequest request)
        {
            // Validasyon (FluentValidation) otomatik devreye girer.
            // Eğer request.UserDescription boşsa veya 10 karakterden kısaysa 
            // sistem otomatik 400 Bad Request döner.

            var response = await _mediator.Send(request);

            // Handler içinde zaten try-catch ile güvenli dönüş (boş liste vb.) 
            // ayarladığımız için direkt OK dönebiliriz.
            return Ok(response);
        }

        // -------------------------------------------------------------------------
        // ETKİNLİK ÖNERİSİ (Bunu da aynı mantıkla güncelleyeceksin)
        // -------------------------------------------------------------------------

        // Kullanıcının moduna göre etkinlik önerir.
        // POST: api/recommendations/events
        [HttpPost("events")]
        public async Task<IActionResult> GetRecommendedEvents([FromBody] GetRecommendedEventRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}