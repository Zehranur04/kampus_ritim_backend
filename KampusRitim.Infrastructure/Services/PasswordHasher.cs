using KampusRitim.Application.Interfaces;
using System.Security.Cryptography; // SHA256 için
using System.Text;


namespace KampusRitim.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        // UYARI: Bu SHA256 yöntemi. 'PasswordHash' kolonunu 'string' (nvarchar(60)) yaparsak, bu metodun içi BCrypt ile değiştirilicek.
        // başta seçtiğimiz 'varbinary(32)' seçimi yüzünden bu yöntemi kullanıyoruz.

        public byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPassword(string password, byte[] passwordHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashToCompare = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return hashToCompare.SequenceEqual(passwordHash);
            }
        }
    }
}
