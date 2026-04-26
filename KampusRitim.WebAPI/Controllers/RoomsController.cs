using KampusRitim.Application.Features.UseCases.Rooms.ApproveReservation; // Yeni klasörümüz
using KampusRitim.Application.Features.UseCases.Rooms.CreateRoomReservation;
using KampusRitim.Application.Features.UseCases.Rooms.GetAllRoom;
using KampusRitim.Application.Features.UseCases.Rooms.GetPendingReservation;
using KampusRitim.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public RoomsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        // 1. GET: Tüm Odaları Listele
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var query = new GetAllRoomRequest();
                var result = await _mediator.Send(query);

                if (result.Success)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }

        // 2. POST: Rezervasyon Talebi Oluştur (Öğrenci)
        [HttpPost("request-reservation")]
        public async Task<IActionResult> RequestReservation([FromBody] CreateRoomReservationRequest request)
        {
            try
            {
                var userId = _currentUserService.UserId;
                if (!userId.HasValue)
                    return Unauthorized(new { Success = false, Message = "Rezervasyon oluşturmak için giriş yapmalısın." });

                // Always bind reservation to the authenticated user.
                request.UserId = userId.Value;

                var result = await _mediator.Send(request);
                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                // Validator hataları veya "Oda dolu" hatası burada yakalanır
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        // 3. PUT: Rezervasyonu Onayla (Admin)
        [HttpPut("approve-reservation")]
        public async Task<IActionResult> ApproveReservation([FromBody] ApproveReservationRequest request)
        {
            try
            {
                var result = await _mediator.Send(request);
                if (result.Success)
                {
                    return Ok(result);
                }

                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }

        // 4. GET: Onay Bekleyen Rezervasyonlar (Admin Panel)
        [HttpGet("pending-reservations")]
        public async Task<IActionResult> GetPendingReservations([FromQuery] string? adminEmail = null)
        {
            try
            {
                var query = new GetPendingReservationRequest { AdminEmail = adminEmail };
                var result = await _mediator.Send(query);
                if (result.Success)
                {
                    return Ok(result);
                }
                return Unauthorized(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
        }
    }
}