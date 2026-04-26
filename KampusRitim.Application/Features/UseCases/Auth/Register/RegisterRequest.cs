using KampusRitim.Domain.Enums;
using MediatR;

namespace KampusRitim.Application.UseCases.Auth.Register
{
    // Eski "RegisterRequestDto"nun yerini aldı.
    public class RegisterRequest : IRequest<RegisterResponse>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public ClassLevel? ClassLevel { get; set; }
    }
}