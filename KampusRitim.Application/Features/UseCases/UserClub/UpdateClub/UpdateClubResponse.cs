namespace KampusRitim.Application.Features.UseCases.UserClub.UpdateClub
{
    public class UpdateClubResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        // Güncellenmiş veriyi de geri dönmek iyi bir pratiktir (Frontend'de listeyi yenilemek için)
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
