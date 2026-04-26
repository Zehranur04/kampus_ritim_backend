using MediatR;
using KampusRitim.Application.Interfaces.Repositories;
using KampusRitim.Application.Interfaces; // IPasswordHasher, ITokenGenerator

namespace KampusRitim.Application.UseCases.Auth.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;

        public LoginCommandHandler(IUserRepository userRepo, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            // 1. Kullanıcıyı bul
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new LoginResponse {
                    IsSuccess = false, 
                    Message = "Geçersiz e-posta veya şifre." 
                };
            }

            // 2. Şifre Doğrulama (Senin yapına göre: string vs byte[])
            // user.PasswordHash veritabanından gelen byte[] dizisidir.
            var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return new LoginResponse { 
                    IsSuccess = false,
                    Message = "Geçersiz e-posta veya şifre."
                };
            }

            // 3. Token Üretme
            var token = _tokenGenerator.GenerateToken(user);

            // 4. Başarılı Dönüş
            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Giriş başarılı.",
                Token = token,
                UserName = $"{user.Name} {user.Surname}"
            };
        }
    }
}