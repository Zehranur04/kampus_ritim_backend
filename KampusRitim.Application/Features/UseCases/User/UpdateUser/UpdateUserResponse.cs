using KampusRitim.Application.Features.Dtos;

namespace KampusRitim.Application.UseCases.User.UpdateUser
{
    public class UpdateUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Güncelleme sonrası frontend'deki ekranı tazelemek için 
        // kullanıcının son halini (şifresiz olarak) dönüyoruz.
        public UserDto? User { get; set; }
    }
}