using System.Threading.Tasks;
using Hackathon.Abstraction.Audit;
using Hackathon.API.Consumers;
using Hackathon.BL.Audit;
using Hackathon.Common.Models.Audit;
using Hackathon.Tests.Integration.Base;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Hackathon.Tests.Integration.MessageBus;

public class MessageBusTests: BaseIntegrationTest
{
    public MessageBusTests(TestWebApplicationFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task AuditEventConsumer_Consume_Success()
    {
        await using var provider = new ServiceCollection()
            .AddScoped<IAuditEventHandler>(_=> new AuditEventHandler(AuditRepository, UserRepository))
            .AddScoped<ILogger<AuditEventConsumer>>(_=>new Logger<AuditEventConsumer>(LoggerFactory))
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<AuditEventConsumer>();
            })
            .BuildServiceProvider(true);

        var harness = provider.GetRequiredService<ITestHarness>();
        await harness.Start();

        await harness.Bus.Publish(new AuditEventModel(AuditEventType.Created));

        Assert.True(await harness.Published.Any<AuditEventModel>(filter => 
            filter.Context.Message.Type == AuditEventType.Created));
        
        Assert.True(await harness.Consumed.Any<AuditEventModel>(filter => 
            filter.Context.Message.Type == AuditEventType.Created));
        
        var consumerHarness = harness.GetConsumerHarness<AuditEventConsumer>();
        Assert.True(await consumerHarness.Consumed.Any<AuditEventModel>(filter => 
            filter.Context.Message.Type == AuditEventType.Created));
    }
}