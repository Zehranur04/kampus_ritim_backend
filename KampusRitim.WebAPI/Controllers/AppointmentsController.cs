using KampusRitim.Application.Features.UseCases.Appointments.CreateAppointment;
using KampusRitim.Application.Features.UseCases.Appointments.DeleteAppointment;
using KampusRitim.Application.Features.UseCases.Appointments.GetProfessorAppointment;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppointmentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. Hocanın Dolu Saatlerini Getir
        [HttpGet("professor/{id}/slots")]
        public async Task<IActionResult> GetProfessorSlots(int id)
        {
            try
            {
                var query = new GetProfessorAppointmentRequest { ProfessorId = id };
                var result = await _mediator.Send(query);

                // Response içindeki Success durumuna göre cevap dönüyoruz
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

        // 2. Randevu Al (Create)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
        {
            try
            {
                var result = await _mediator.Send(request);

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

        // 4. Kullanıcının Randevularını Getir (My Appointments)
        [HttpGet("my")]
        public async Task<IActionResult> GetMyAppointments()
        {
            try
            {
                var query = new KampusRitim.Application.Features.UseCases.Appointments.GetMyAppointments.GetMyAppointmentsRequest();
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

        // 3. Randevu İptal Et (Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var request = new DeleteAppointmentRequest { AppointmentId = id };
                var result = await _mediator.Send(request);

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
    }
}