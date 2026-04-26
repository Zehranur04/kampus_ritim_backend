namespace KampusRitim.Application.Features.Dtos
{
    public record ClubDto(
        int Id,
        string Name,
        string? Description,
        string? ProfileImageUrl,
        DateTime CreatedAt
    );
}
