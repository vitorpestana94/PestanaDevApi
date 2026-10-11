namespace PestanaDevApi.Interfaces.Services
{
    public interface IBoardHubConnectionService
    {
        /// <summary>
        /// Registers an open SignalR connection of the user.
        /// </summary>
        void AddConnection(Guid userId, string connectionId);

        /// <summary>
        /// Removes a closed SignalR connection of the user.
        /// </summary>
        void RemoveConnection(Guid userId, string connectionId);

        /// <summary>
        /// Retrieves all open SignalR connections of the user.
        /// </summary>
        IEnumerable<string> GetConnections(Guid userId);
    }
}
