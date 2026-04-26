namespace KampusRitim.Application.Interfaces
{
    public interface ICurrentUserService
    {
        // O anki istekteki Token'ın içindeki User Id'yi döner
        int? UserId { get; }
    }
}