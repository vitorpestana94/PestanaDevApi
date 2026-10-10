using System.Collections.Concurrent;
using PestanaDevApi.Interfaces.Services;

namespace PestanaDevApi.Services
{
    /// <summary>
    /// Keeps track of the open SignalR connections of each user (one user can have many tabs/devices).
    /// It's needed to kick a removed member out of the board group, since groups only work with connection ids.
    /// Must be registered as Singleton.
    /// </summary>
    public class BoardHubConnectionService: IBoardHubConnectionService
    {
        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<string, byte>> _connections = new();

        public void AddConnection(Guid userId, string connectionId)
        {
            _connections.GetOrAdd(userId, _ => new()).TryAdd(connectionId, 0);
        }

        public void RemoveConnection(Guid userId, string connectionId)
        {
            if (!_connections.TryGetValue(userId, out ConcurrentDictionary<string, byte>? userConnections))
                return;

            userConnections.TryRemove(connectionId, out _);

            if (userConnections.IsEmpty)
                _connections.TryRemove(userId, out _);
        }

        public IEnumerable<string> GetConnections(Guid userId)
        {
            if (!_connections.TryGetValue(userId, out ConcurrentDictionary<string, byte>? userConnections))
                return [];

            return userConnections.Keys.ToList();
        }
    }
}
