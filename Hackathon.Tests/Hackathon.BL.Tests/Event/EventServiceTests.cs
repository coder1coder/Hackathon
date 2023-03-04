using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.Abstraction.Notification;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.BL.Event;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.EventLog;
using Hackathon.Common.Models.EventStage;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MassTransit;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Hackathon.BL.Tests.Event;

public class EventServiceTests: BaseUnitTest
{
    private Mock<IValidator<EventCreateParameters>> _createValidatorMock = new();
    private Mock<IValidator<EventUpdateParameters>> _updateValidatorMock = new();
    private Mock<IValidator<GetListParameters<EventFilter>>> _getListValidatorMock = new();

    private Mock<ITeamService> _teamServiceMock = new();
    private Mock<INotificationService> _notificationServiceMock = new ();
    private Mock<IMessageHub<EventStatusChangedIntegrationEvent>> _integrationEventHubMock = new();

    private Mock<IEventRepository> _eventRepositoryMock = new();
    private Mock<IUserRepository> _userRepositoryMock = new();

    private Mock<IBus> _busMock = new();

    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);

        _createValidatorMock = new Mock<IValidator<EventCreateParameters>>();
        _updateValidatorMock = new Mock<IValidator<EventUpdateParameters>>();
        _getListValidatorMock = new Mock<IValidator<GetListParameters<EventFilter>>>();

        _teamServiceMock = new Mock<ITeamService>();
        _notificationServiceMock = new Mock<INotificationService>();

        _eventRepositoryMock = new Mock<IEventRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _busMock = new Mock<IBus>();
        _integrationEventHubMock = new Mock<IMessageHub<EventStatusChangedIntegrationEvent>>();

        _eventRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<EventCreateParameters>()))
            .ReturnsAsync(createdId);

        _busMock.Setup(x => x.Publish(It.IsAny<EventLogModel>(), default))
            .Returns(Task.CompletedTask);

        var service = new EventService(
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _getListValidatorMock.Object,
            _eventRepositoryMock.Object,
            _teamServiceMock.Object,
            _userRepositoryMock.Object,
            _notificationServiceMock.Object,
            _busMock.Object,
            _integrationEventHubMock.Object
        );

        //act
        var result = await service.CreateAsync(new EventCreateParameters
        {
            Stages = new List<EventStageModel>()
        });

        //assert
        Assert.NotNull(result);
        result.Data.Should().Be(createdId);
    }
}
