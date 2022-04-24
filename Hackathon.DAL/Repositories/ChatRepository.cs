using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Newtonsoft.Json;
using StackExchange.Redis;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Hackathon.DAL.Repositories;

public class ChatRepository: RedisRepository, IChatRepository
{
    public ChatRepository(
        IConnectionMultiplexer connectionMultiplexer):base(connectionMultiplexer)
    {
    }
    
    public async Task Add(ChatMessageEntity entity)
    {
        var value = JsonSerializer.Serialize(entity);
        await RedisDatabase.SetAddAsync(GetRedisKey(entity), new RedisValue(value));
    }

    public async Task<BaseCollectionModel<TeamChatMessage>> GetTeamChatMessages(long teamId, int offset = 0, int limit = 300)
    {
        var items = RedisDatabase.SetScan(GetRedisKey(new ChatMessageEntity
        {
            TeamId = teamId,
            Context = ChatMessageContext.TeamChat
        }))
        .Select(x => 
            JsonConvert.DeserializeObject<ChatMessageEntity>(x))
        .ToArray();

        var totalCount = items.Length;

        return await Task.FromResult(new BaseCollectionModel<TeamChatMessage>
        {
            Items = items
                .Skip(offset)
                .Take(300)
                .Select(x=>x.ToTeamChatMessage())
                .OrderBy(x=>x.Timestamp)
                .ToArray(),
            
            TotalCount = totalCount
        });
    }

    private static string GetRedisKey(ChatMessageEntity entity)
    {
        const string redisKeyPrefix = "chat:";
        
        return entity.Context switch
        {
            ChatMessageContext.TeamChat => new RedisKey($"{redisKeyPrefix}team:{entity.TeamId}"),
            _ => throw new ArgumentOutOfRangeException(nameof(entity.Context), entity.Context, null)
        };
    }
}