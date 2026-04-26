using KampusRitim.Application.Features.Dtos;
using KampusRitim.Application.Interfaces; // IPasswordHasher namespace'i
using KampusRitim.Application.Interfaces.Repositories;
using MediatR;

namespace KampusRitim.Application.UseCases.User.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;

        public UpdateUserHandler(IUserRepository userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            // 1. ADIM: Kullanıcıyı Bulma Kontrolü
            var user = await _userRepo.GetByIdAsync(request.Id);

            if (user == null)
            {
                // HATA 1: Kullanıcı Yok
                return new UpdateUserResponse
                {
                    IsSuccess = false,
                    Message = $"ID'si {request.Id} olan kullanıcı bulunamadı.",
                    User = null
                };
            }

            // 2. ADIM: Email Çakışma Kontrolü
            // Eğer kullanıcı emailini değiştirmek istiyorsa (eski ile yeni farklıysa) kontrol et
            if (request.Email != user.Email)
            {
                // Bu emaili kullanan BAŞKA biri var mı?
                var existingUserWithEmail = await _userRepo.GetByEmailAsync(request.Email);

                if (existingUserWithEmail != null)
                {
                    // HATA 2: Email zaten kullanımda
                    return new UpdateUserResponse
                    {
                        IsSuccess = false,
                        Message = $"'{request.Email}' adresi başka bir kullanıcı tarafından kullanılıyor.",
                        User = null
                    };
                }

                // Sorun yoksa emaili güncelle
                user.Email = request.Email;
            }

            // 3. ADIM: Şifre Güncelleme (Varsa)
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                // Senin IPasswordHasher yapına göre (byte[] dönüyor)
                user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
            }

            // 4. ADIM: Diğer Bilgileri Güncelle
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.Faculty = request.Faculty;
            user.Department = request.Department;
            user.ClassLevel = request.ClassLevel;

            // 5. ADIM: Veritabanına Kaydet
            await _userRepo.UpdateAsync(user);

            // 6. ADIM: Başarılı Dönüş (Happy Path)
            var userDto = new UserDto(
                user.Id,
                user.Name,
                user.Surname,
                user.Email,
                user.Faculty,
                user.Department,
                user.ClassLevel
            );

            return new UpdateUserResponse
            {
                IsSuccess = true,
                Message = "Profil bilgileri başarıyla güncellendi.",
                User = userDto
            };
        }
    }
}