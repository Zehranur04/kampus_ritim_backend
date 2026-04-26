using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.User.GetUserByEmail
{
    public class GetUserByEmailResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Kullanıcı bulunursa dolu, bulunamazsa null
        public UserDto? User { get; set; }
    }
}