namespace KampusRitim.Application.Interfaces
{
    // Şifreleri güvenli bir şekilde hash'lemek ve doğrulamak için kullanılacak 
    public interface IPasswordHasher
    {
        // Verilen düz metin şifreyi hash'ler.
        byte[] HashPassword(string password);

        // Verilen düz metin şifreyi, veritabanındaki hash ile karşılaştırır.
        // returns olarak Şifre doğruysa true, yanlışsa false döner
        bool VerifyPassword(string password, byte[] passwordHash);
    }
}
