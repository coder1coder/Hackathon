using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hackathon.DAL.Repositories.Chat;

public abstract class ChatRepository<TChatMessage>: RedisRepository, IChatRepository<TChatMessage>
where TChatMessage: class, IChatMessage
{
    protected abstract Expression<Func<TChatMessage, long>> EntityIdentityKey { get; }

    protected ChatRepository(IConnectionMultiplexer connectionMultiplexer):base(connectionMultiplexer)
    {
    }

    public async Task AddMessageAsync(TChatMessage chatMessage)
    {
        var chatMessageAsString = JsonSerializer.Serialize(chatMessage, chatMessage.GetType());
        await RedisDatabase.SetAddAsync(GetRedisKey(GetMessageId(chatMessage)), new RedisValue(chatMessageAsString));
    }

    public async Task<BaseCollection<TChatMessage>> GetMessagesByKeyAsync(string key, int offset = 0, int limit = 300)
    {
        var items = RedisDatabase.SetScan(GetRedisKey(key))
            .Select(x => JsonSerializer.Deserialize<TChatMessage>(x))
            .ToArray();

        var totalCount = items.Length;

        return await Task.FromResult(new BaseCollection<TChatMessage>
        {
            Items = items
                .Skip(offset)
                .Take(limit)
                .OrderBy(x=>x.Timestamp)
                .ToArray(),

            TotalCount = totalCount
        });
    }

    private string GetMessageId(TChatMessage chatMessage)
    {
        var member = ((MemberExpression)EntityIdentityKey.Body).Member;

        var prop = member as PropertyInfo;
        return prop?.GetValue(chatMessage)?.ToString();
    }

    private static string GetRedisKey(string messageId) => $"chat:{typeof(TChatMessage).Name}:{messageId}";
}
