using KampusRitim.Application.Interfaces; // IPasswordHasher
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.Auth.Register
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterRequestHandler(IUserRepository userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            // 1. Email Kontrolü (Repository'den gelen user null değilse email alınmış demektir)
            var existingUser = await _userRepo.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new RegisterResponse { IsSuccess = false, Message = "Bu email adresi zaten kullanılıyor." };
            }

            // 2. Şifreleme (Senin byte[] dönen metodun)
            var passwordHash = _passwordHasher.HashPassword(request.Password);

            // 3. Entity Oluşturma (User + Otomatik Boş Profil)
            var newUser = new KampusRitim.Domain.Entity.User
            {
                Name = request.Name,
                Surname = request.Surname,
                Email = request.Email,
                PasswordHash = passwordHash, // Hashlenmiş şifre
                Faculty = request.Faculty,
                Department = request.Department,
                ClassLevel = request.ClassLevel,
                CreatedAt = DateTime.UtcNow,

                // OTOMATİK PROFİL (Senin İstediğin Kısım)
                Profile = new KampusRitim.Domain.Entity.Profile
                {
                        // AYNI VERİLER BURAYA DA KOPYALANIYOR
                        Name = request.Name,
                        Surname = request.Surname,
                        Faculty = request.Faculty,
                        Department = request.Department,
                        ClassLevel = request.ClassLevel,

                        // Profile özel diğer alanlar
                    Bio = string.Empty, // Boş başlasın, sonra günceller
                    ProfileImageUrl = null, // Resim yoksa null geçebilirsin
                    CreatedAt = DateTime.UtcNow
                }
            };

            // 4. Kayıt
            await _userRepo.AddAsync(newUser);

            return new RegisterResponse
            {
                IsSuccess = true,
                Message = "Kullanıcı kaydı başarıyla oluşturuldu."
            };
        }
    }
}