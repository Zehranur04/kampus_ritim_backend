using KampusRitim.Application.Features.UseCases.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var result = await _mediator.Send(new GetUserNotificationsQuery());
            return Ok(result);
        }

        [HttpPut("read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _mediator.Send(new MarkAsReadCommand(id));
            if (!result) return NotFound();
            return Ok(new { Message = "Bildirim okundu." });
        }
    }
}
