using KampusRitim.Application.Features.UseCases.Vote.CreateVote; // CreateVote namespace'i
using KampusRitim.Application.Features.UseCases.Vote.GetEventRating;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class VotesController : ControllerBase
        {
            private readonly IMediator _mediator;

            public VotesController(IMediator mediator)
            {
                _mediator = mediator;
            }

            // POST api/votes
            [HttpPost]
            public async Task<IActionResult> CreateVote([FromBody] CreateVoteRequest request)
            {
                var result = await _mediator.Send(request);
                return Ok(result);
            }

            // GET api/votes/{eventId}
            [HttpGet("{eventId}")]
            public async Task<IActionResult> GetEventRating(int eventId)
            {
                var query = new GetEventRatingRequest(eventId);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
        }
    }

