namespace KampusRitim.Application.Features.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
        public bool HasProjector { get; set; }
    }
}
