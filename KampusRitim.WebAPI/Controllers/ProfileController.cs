using KampusRitim.Application.UseCases.Profile.DeleteProfile;
using KampusRitim.Application.UseCases.Profile.GetAllProfile;
using KampusRitim.Application.UseCases.Profile.GetMyProfile;
using KampusRitim.Application.UseCases.Profile.GetProfileById;
using KampusRitim.Application.UseCases.Profile.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
{
    [Route("api/profiles")] // Standart gereği çoğul yaptım (api/profiles)
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ---------------------------------------------------------
        // KİŞİSEL İŞLEMLER (Sadece Giriş Yapan Kullanıcı İçin)
        // ---------------------------------------------------------

        // GET: api/profiles/me
        // Token sahibinin kendi profilini getirir
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            // ID göndermiyoruz, Handler token'dan okuyor.
            var response = await _mediator.Send(new GetMyProfileRequest());

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }

        // PUT: api/profiles/me
        // Token sahibinin kendi profilini günceller
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
        {
            // Kullanıcı ID'sini command'e işlemeye gerek yok, handler token'dan okuyacak.
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }

        // ---------------------------------------------------------
        // GENEL / ADMIN İŞLEMLERİ
        // ---------------------------------------------------------

        // GET: api/profiles
        // Tüm profilleri listeler
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllProfileRequest());
            return Ok(response);
        }

        // GET: api/profiles/5
        // Başkasının profiline bakmak için (ID ile)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _mediator.Send(new GetProfileByIdRequest { Id = id });

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }

        // DELETE: api/profiles/5
        // Bir profili silmek için (Genelde Admin yetkisi istenir ama şimdilik Authorize yeterli)
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteProfileRequest { Id = id });

            if (!response.IsSuccess)
                return NotFound(response);

            return Ok(response);
        }
    }
}