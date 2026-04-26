using KampusRitim.Application.Interfaces;
using KampusRitim.Domain.Entity;
using Microsoft.Extensions.Configuration; // IConfiguration için
using Microsoft.IdentityModel.Tokens; // SymmetricSecurityKey için
using System.IdentityModel.Tokens.Jwt; // JwtSecurityToken için
using System.Security.Claims; // Claims için
using System.Text;

namespace KampusRitim.Infrastructure.Services
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Gizli anahtarı al ve byte[] dizisine çevir
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            // Şifreleme algoritmasını (HmacSha256) belirle
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token'ın içine gömeceğimiz bilgiler
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Subject (Kullanıcı ID)
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.Name + " " + user.Surname),
                // Role claims (use standard ClaimTypes.Role + a numeric fallback)
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("role", ((int)user.Role).ToString())
            };

            // Token'ı oluştur
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3), // Token 3 saat geçerli
                signingCredentials: creds
            );

            // Token'ı metin (string) formatına çevir ve döndür
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}