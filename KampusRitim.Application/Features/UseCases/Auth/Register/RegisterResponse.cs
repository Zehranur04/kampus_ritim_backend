namespace KampusRitim.Application.UseCases.Auth.Register
{
    public class RegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Eğer kayıt sonrası direkt token döneceksen buraya Token property'si eklersin.
        // Ama tavsiyem: Kayıt -> Başarılı -> Login Ekranına Yönlendir.
    }
}