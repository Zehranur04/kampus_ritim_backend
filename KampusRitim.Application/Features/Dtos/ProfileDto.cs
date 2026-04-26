using KampusRitim.Domain.Enums;
using System.Text.Json.Serialization;

namespace KampusRitim.Application.Features.Dtos
{
    public record ProfileDto(
        // --- Constructor Parametreleri ---
        string Name,
        string Surname,
        string Email,
        string? Faculty,
        string? Department,
        string Bio,
        string? ProfileImageUrl,

        // Enum'ı string'e çevirmek için attribute
        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        ClassLevel? ClassLevel,

        int AttendingEventsCount
    )
    {
        // --- Hesaplanan Özellikler (Computed Properties) ---
        // Record parametrelerini kullanarak otomatik üretilir
        public string FullName => $"{Name} {Surname}";
    }
}