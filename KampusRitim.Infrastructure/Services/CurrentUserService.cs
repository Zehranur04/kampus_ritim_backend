using System.Security.Claims;
using System.IO;
using KampusRitim.Application.Interfaces;
using Microsoft.AspNetCore.Http; // HttpContextAccessor için gerekli

namespace KampusRitim.Infrastructure
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                // Kullanıcı giriş yapmamışsa null döner
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null) return null;

                // Token içindeki "nameid" veya "sub" claim'ini okur
                var idClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst("sub");

                // Debug: log claim values to file for troubleshooting
                try
                {
                    var logPath = Path.Combine(AppContext.BaseDirectory, "currentuser_debug.log");
                    var claims = user.Claims.Select(c => $"{c.Type}={c.Value}");
                    var content = $"[{DateTime.UtcNow:O}] Claims: {string.Join(';', claims)}\n";
                    File.AppendAllText(logPath, content);
                }
                catch
                {
                    // Ignore logging errors
                }

                if (idClaim != null && int.TryParse(idClaim.Value, out int userId))
                {
                    return userId;
                }

                return null;
            }
        }
    }
}