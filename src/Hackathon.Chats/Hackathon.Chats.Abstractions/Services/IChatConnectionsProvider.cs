using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hackathon.Chats.Abstractions.Services;

public interface IChatConnectionsProvider
{
    Task SaveUserConnectionIdAsync(long userId, string connectionId, CancellationToken cancellationToken = default);
    Task RemoveUserConnectionIdAsync(long userId, string connectionId, CancellationToken cancellationToken = default);
    Task<HashSet<string>> GetUserConnectionIdsAsync(long userId, CancellationToken cancellationToken = default);
}
