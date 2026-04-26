namespace KampusRitim.Application.UseCases.Auth.Login
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public string? Token { get; set; }
        public string? UserName { get; set; }
    }
}