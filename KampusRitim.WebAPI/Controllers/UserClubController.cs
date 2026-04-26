using KampusRitim.Application.Features.UseCases.GetMyClub;
using KampusRitim.Application.Features.UseCases.UserClub.JoinClub;
using KampusRitim.Application.Features.UseCases.UserClub.LeaveClub;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Bu sınıftaki tüm metodlar geçerli bir JWT Token ister
    public class UserClubController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserClubController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("join")]
        public async Task<IActionResult> Join([FromBody] JoinClubRequest request)
        {
            var result = await _mediator.Send(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("mine")]
        public async Task<IActionResult> GetMyClubs()
        {
            var result = await _mediator.Send(new GetMyClubRequest());
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("leave/{clubId}")]
        public async Task<IActionResult> Leave(int clubId)
        {
            // URL'den gelen clubId'yi request nesnesine paketleyip gönderiyoruz
            var result = await _mediator.Send(new LeaveClubRequest { ClubId = clubId });
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}