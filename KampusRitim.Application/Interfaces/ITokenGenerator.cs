using KampusRitim.Domain.Entity;

namespace KampusRitim.Application.Interfaces
{
    // Başarılı giriş sonrası kullanıcı için JWT üretir.
    public interface ITokenGenerator
    {
        // Verilen User nesnesine ait bilgileri (Id, Email, Role) içeren bir JWT string'i oluşturur.
        string GenerateToken(User user);
    }
}
