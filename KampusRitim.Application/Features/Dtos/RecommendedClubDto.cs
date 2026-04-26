namespace KampusRitim.Application.Features.Dtos
{
    public class RecommendedClubDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Buradaki Description, kulübün orijinal açıklaması değil,
        // Yapay zekanın "Bunu neden seçtim" açıklaması olacak.
        public string Description { get; set; } = string.Empty;

        public string CategoryName { get; set; } = string.Empty;
    }
}
