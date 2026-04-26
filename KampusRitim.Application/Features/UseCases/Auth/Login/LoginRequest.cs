using MediatR;

namespace KampusRitim.Application.UseCases.Auth.Login
{
    // Cevap olarak LoginResponse dönecek
    public class LoginRequest : IRequest<LoginResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}