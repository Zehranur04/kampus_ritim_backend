using KampusRitim.Domain.Enums;
using MediatR;

namespace KampusRitim.Application.UseCases.Profile.UpdateProfile
{
    public class UpdateProfileRequest : IRequest<UpdateProfileResponse>
    {
        // Kullanıcı ID'sini Token'dan alacağımız için buraya koymuyoruz.

        // --- User Tablosuna Gidecekler ---
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public ClassLevel? ClassLevel { get; set; }

        // --- Profile Tablosuna Gidecekler ---
        public string Bio { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }
}