using System.Threading.Tasks;
using Hackathon.Abstraction.EventLog;
using Hackathon.API.Consumers;
using Hackathon.BL.EventLog;
using Hackathon.Common.Models.EventLog;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Hackathon.Tests.Integration.MessageBus;

public class MessageBusTests : BaseIntegrationTest
{
    public MessageBusTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task EventLogConsumer_Consume_Success()
    {
        await using var provider = new ServiceCollection()
            .AddScoped<IEventLogHandler>(_ => new EventLogHandler(EventLogService, NullLogger<EventLogHandler>.Instance))
            .AddScoped<ILogger<EventLogConsumer>>(_ => NullLogger<EventLogConsumer>.Instance)
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<EventLogConsumer>();
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        await harness.Bus.Publish(new EventLogModel(EventLogType.Created));

        Assert.True(await harness.Published.Any<EventLogModel>(filter =>
            filter.Context.Message.LogType == EventLogType.Created));

        Assert.True(await harness.Consumed.Any<EventLogModel>(filter =>
            filter.Context.Message.LogType == EventLogType.Created));

        var consumerHarness = harness.GetConsumerHarness<EventLogConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<EventLogModel>(filter =>
            filter.Context.Message.LogType == EventLogType.Created));
    }
}
