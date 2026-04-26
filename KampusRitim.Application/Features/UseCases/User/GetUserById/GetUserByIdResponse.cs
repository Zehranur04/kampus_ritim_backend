using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.User.GetUserById
{
    public class GetUserByIdResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Aranan kullanıcı (Bulunamazsa null olabilir)
        public UserDto? User { get; set; }
    }
}