using KampusRitim.Application.UseCases.Auth.Login;    // LoginCommand namespace
using KampusRitim.Application.UseCases.Auth.Register; // RegisterCommand namespace
using KampusRitim.Application.Interfaces;
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KampusRitim.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserRepository _userRepository;

        public AuthController(IMediator mediator, ICurrentUserService currentUser, IUserRepository userRepository)
        {
            _mediator = mediator;
            _currentUser = currentUser;
            _userRepository = userRepository;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return BadRequest(response); // Mesaj: "Email kullanımda" vb.

            return Ok(response);
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _mediator.Send(request);

            if (!response.IsSuccess)
                return Unauthorized(response); // 401: Giriş başarısız

            return Ok(response);
        }

        // GET api/auth/me
        // JWT token sahibinin backend kullanıcı bilgisini (role dahil) döner
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            if (!_currentUser.UserId.HasValue)
            {
                return Unauthorized(new { IsSuccess = false, Message = "Giriş yapılmamış." });
            }

            var user = await _userRepository.GetByIdAsync(_currentUser.UserId.Value);
            if (user == null)
            {
                return Unauthorized(new { IsSuccess = false, Message = "Kullanıcı bulunamadı." });
            }

            return Ok(new
            {
                IsSuccess = true,
                Id = user.Id,
                Email = user.Email,
                Role = user.Role
            });
        }
    }
}

