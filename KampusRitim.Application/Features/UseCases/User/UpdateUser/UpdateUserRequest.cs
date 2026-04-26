using KampusRitim.Domain.Enums;
using MediatR;

namespace KampusRitim.Application.UseCases.User.UpdateUser
{
    public class UpdateUserRequest : IRequest<UpdateUserResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!; // Email eklendi

        // Şifre boş gelirse değiştirmiyoruz, dolu gelirse güncelliyoruz.
        public string? NewPassword { get; set; }

        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public ClassLevel? ClassLevel { get; set; }
    }
}