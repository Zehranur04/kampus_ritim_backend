using KampusRitim.Application.Features.Dtos; // UserDto burada

namespace KampusRitim.Application.UseCases.User.GetAllUsers
{
    public class GetAllUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Kullanıcı listesi
        public IEnumerable<UserDto> Users { get; set; } = new List<UserDto>();
    }
}