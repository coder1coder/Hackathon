using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Hackathon.Chats.Infrastructure.IntegrationEvents;

public class ChatConnectionsProvider: IChatConnectionsProvider
{
    private const char ConnectionsSeparator = ';';
    private readonly Func<long, string> _userConnectionsCacheKeyResolver = userId => $"user-{userId}-chat-connection-ids";
    private readonly IDistributedCache _cache;

    public ChatConnectionsProvider(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SaveUserConnectionIdAsync(long userId, string connectionId, CancellationToken cancellationToken = default)
    {
        
        var cacheKey = _userConnectionsCacheKeyResolver.Invoke(userId);
        var cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken) ?? string.Empty;
        var connections = cachedValue.Split(ConnectionsSeparator).ToHashSet();

        if (!connections.Add(connectionId))
        {
            return;
        }

        var newCacheValue = string.Join(ConnectionsSeparator, connections);
        await _cache.SetStringAsync(cacheKey, newCacheValue, cancellationToken);
    }
    
    public async Task RemoveUserConnectionIdAsync(long userId, string connectionId, CancellationToken cancellationToken = default)
    {
        var cacheKey = _userConnectionsCacheKeyResolver.Invoke(userId);
        var cachedValue = await _cache.GetStringAsync(cacheKey, cancellationToken) ?? string.Empty;
        var connections = cachedValue.Split(ConnectionsSeparator).ToHashSet();

        if (!connections.Remove(connectionId))
        {
            return;
        }

        if (connections.Any())
        {
            var newCacheValue = string.Join(ConnectionsSeparator, connections);
            await _cache.SetStringAsync(cacheKey, newCacheValue, cancellationToken);
        }
        else
        {
            await _cache.RemoveAsync(cacheKey, cancellationToken);
        }
    }

    public async Task<HashSet<string>> GetUserConnectionIdsAsync(long userId, CancellationToken cancellationToken = default)
    {
        var cachedValue = await _cache.GetStringAsync(_userConnectionsCacheKeyResolver.Invoke(userId), cancellationToken) ?? string.Empty;
        return cachedValue.Split(ConnectionsSeparator).ToHashSet();
    }
}
