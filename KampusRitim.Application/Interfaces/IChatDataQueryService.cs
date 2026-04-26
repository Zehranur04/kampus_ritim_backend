namespace KampusRitim.Application.Interfaces
{
    public interface IChatDataQueryService
    {
        /// <summary>
        /// Tries to answer the user's message using live database data (events/rooms/reservations).
        /// Returns null if the message is not a supported data query.
        /// </summary>
        Task<string?> TryResolveAsync(string message, int? userId, CancellationToken cancellationToken = default);
    }
}
