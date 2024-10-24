﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hackathon.Infrastructure;

public class MessageBusService: IMessageBusService
{
    private readonly IBus _bus;
    private readonly ILogger<MessageBusService> _logger;

    public MessageBusService(IBus bus, ILogger<MessageBusService> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    public async Task<bool> TryPublish<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            await Publish(message, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Source} Ошибка при попытке опубликовать сообщение в шину. {ErrorMessage}",
                nameof(MessageBusService),
                e.Message);
            return false;
        }
    }
    
    private Task Publish<T>(T message, CancellationToken cancellationToken = default) where T : class
        => _bus.Publish(message, cancellationToken);
}
