using System;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.User;
using Hackathon.Logbook.Abstraction.Handlers;
using Hackathon.Logbook.Abstraction.Models;
using Hackathon.Logbook.Abstraction.Repositories;
using Microsoft.Extensions.Logging;

namespace Hackathon.Logbook.BL.Handlers;

public class EventLogHandler: IEventLogHandler
{
    private readonly IEventLogRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<EventLogHandler> _logger;

    public EventLogHandler(
        ILogger<EventLogHandler> logger, 
        IEventLogRepository repository, 
        IUserRepository userRepository)
    {
        _logger = logger;
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task Handle(EventLogModel eventLogModel)
    {
        try
        {
            if (eventLogModel.UserId.HasValue)
            {
                var user = await _userRepository.GetAsync(eventLogModel.UserId.Value);
                eventLogModel.Username = user?.GetAnyName();
            }
            
            await _repository.AddAsync(eventLogModel);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Source}. Ошибка во время обработки сообщения журнала событий",
                nameof(EventLogHandler));
            throw;
        }
    }
}
