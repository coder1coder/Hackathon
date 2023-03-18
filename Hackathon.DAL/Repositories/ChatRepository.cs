using Hackathon.Common.Abstraction.Chat;
using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Team;
using Newtonsoft.Json;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Hackathon.DAL.Repositories;

public class ChatRepository: RedisRepository, IChatRepository
{
    public ChatRepository(IConnectionMultiplexer connectionMultiplexer):base(connectionMultiplexer)
    {
    }

    public async Task AddMessage<TChatMessage>(TChatMessage chatMessage) where TChatMessage: IChatMessage
    {
        var chatMessageAsString = JsonSerializer.Serialize(chatMessage, chatMessage.GetType());
        await RedisDatabase.SetAddAsync(GetRedisKey(chatMessage), new RedisValue(chatMessageAsString));
    }

    public Task<BaseCollection<TeamChatMessage>> GetTeamChatMessages(long teamId, int offset = 0, int limit = 300)
        => GetMessages<TeamChatMessage>(GetRedisKey(new TeamChatMessage
        {
            TeamId = teamId,
            Type = ChatMessageType.TeamChat
        }), offset, limit);

    private async Task<BaseCollection<TChatMessage>> GetMessages<TChatMessage>(string key, int offset = 0, int limit = 300)
    where TChatMessage: class, IChatMessage
    {
        var items = RedisDatabase.SetScan(key)
            .Select(x => JsonConvert.DeserializeObject<TChatMessage>(x))
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

    private static string GetRedisKey(IChatMessage chatMessage)
    {
        const string redisKeyPrefix = "chat:";

        return chatMessage switch
        {
            TeamChatMessage teamChatMessage => new RedisKey($"{redisKeyPrefix}team:{teamChatMessage.TeamId}"),
            _ => throw new ArgumentOutOfRangeException(nameof(chatMessage.Type), chatMessage.Type, null)
        };
    }
}
