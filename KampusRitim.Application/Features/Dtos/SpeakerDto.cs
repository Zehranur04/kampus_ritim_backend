namespace KampusRitim.Application.Features.Dtos
{
 public record SpeakerDto (
  int Id,
  string Name,
  string Surname,
  string? Bio,
  string? ProfileImageUrl );
    
}
