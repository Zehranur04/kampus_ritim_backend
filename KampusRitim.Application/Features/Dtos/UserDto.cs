using KampusRitim.Domain.Enums;
using System.Text.Json.Serialization;

namespace KampusRitim.Application.Features.Dtos
{
    public record UserDto(
        int Id,
        string Name,
        string Surname,
        string Email,
        string? Faculty,
        string? Department,

        [property: JsonConverter(typeof(JsonStringEnumConverter))]
        ClassLevel? ClassLevel // Artık Enum tipinde
    );
}