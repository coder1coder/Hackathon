using System;
using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Repositories;
using Hackathon.Chats.DAL.Entities;
using Hackathon.Common.Models.Base;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Chats.DAL.Repositories;

public class ChatRepository<TChatMessage>: IChatRepository<TChatMessage> where TChatMessage: class
{
    private readonly ChatsDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ChatType _chatType;

    protected ChatRepository(ChatsDbContext dbContext, IMapper mapper, ChatType chatType)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _chatType = chatType;
    }

    public async Task<Guid> AddMessageAsync(TChatMessage chatMessage)
    {
        var entity = _mapper.Map<TChatMessage, ChatMessageEntity>(chatMessage);
        entity.ChatType = _chatType;
        _dbContext.ChatMessages.Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity.MessageId;
    }

    public async Task<BaseCollection<TChatMessage>> GetMessagesAsync(long chatId, int offset = 0, int limit = 300)
    {
        var entities = await _dbContext
            .ChatMessages.Where(x=>
                x.ChatType == _chatType
                && x.ChatId == chatId)
            .OrderByDescending(x=>x.Timestamp)
            .Skip(offset)
            .Take(limit)
            .Reverse()
            .AsNoTracking()
            .ToArrayAsync();

        return new BaseCollection<TChatMessage>
        {
            Items = _mapper.Map<ChatMessageEntity[], TChatMessage[]>(entities),
            TotalCount = entities.Length
        };
    }

    public async Task<Result<TChatMessage>> GetMessageAsync(Guid messageId)
    {
        var entity = await _dbContext.ChatMessages
            .FirstOrDefaultAsync(x =>
                x.ChatType == _chatType
                && x.MessageId == messageId);

        if (entity is null)
            return Result<TChatMessage>.NotFound("Сообщение не найдено");

        var chatMessage = _mapper.Map<TChatMessage>(entity);

        return Result<TChatMessage>.FromValue(chatMessage);
    }
}
