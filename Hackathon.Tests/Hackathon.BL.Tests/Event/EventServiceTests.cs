using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Hackathon.BL.Event;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.EventLog;
using Hackathon.Common.Models.EventStage;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MassTransit;
using Moq;
using System.Collections.Generic;
using Hackathon.Common.Abstraction.FileStorage;
using Xunit;
using Bogus;

namespace Hackathon.BL.Tests.Event;

public class EventServiceTests: BaseUnitTest
{
    private readonly EventService _service;
    private readonly Mock<IValidator<EventCreateParameters>> _createValidatorMock;
    private readonly Mock<IValidator<EventUpdateParameters>> _updateValidatorMock;
    private readonly Mock<IValidator<Common.Models.GetListParameters<EventFilter>>> _getListValidatorMock;
    private readonly Mock<ITeamService> _teamServiceMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<IMessageHub<EventStatusChangedIntegrationEvent>> _integrationEventHubMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IBus> _busMock;

    public EventServiceTests()
    {
        _createValidatorMock = new Mock<IValidator<EventCreateParameters>>();
        _updateValidatorMock = new Mock<IValidator<EventUpdateParameters>>();
        _getListValidatorMock = new Mock<IValidator<Common.Models.GetListParameters<EventFilter>>>();
        _teamServiceMock = new Mock<ITeamService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _busMock = new Mock<IBus>();
        _integrationEventHubMock = new Mock<IMessageHub<EventStatusChangedIntegrationEvent>>();
        
        _service = new EventService(
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _getListValidatorMock.Object,
            _eventRepositoryMock.Object,
            _teamServiceMock.Object,
            _userRepositoryMock.Object,
            _notificationServiceMock.Object,
            _busMock.Object,
            _integrationEventHubMock.Object,
            _fileStorageServiceMock.Object
        );
    }
    
    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);
        
        _eventRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<EventCreateParameters>()))
            .ReturnsAsync(createdId);

        _busMock.Setup(x => x.Publish(It.IsAny<EventLogModel>(), default))
            .Returns(Task.CompletedTask);
        
        //act
        var result = await _service.CreateAsync(new EventCreateParameters
        {
            Stages = new List<EventStageModel>()
        });

        //assert
        Assert.NotNull(result);
        result.Data.Should().Be(createdId);
    }

    [Fact]
    public async Task Update_Should_Return_Result_Success()
    {
        //arrange
        var eventUpdateParameters = new Faker<EventUpdateParameters>()
            .RuleFor(x => x.Id, f => f.Random.Long(1, 10))
            .Generate();
        
        _eventRepositoryMock.Setup(x => x.ExistsAsync(eventUpdateParameters.Id))
            .Returns(Task.FromResult(true));
            
        _eventRepositoryMock.Setup(x => x.UpdateAsync(eventUpdateParameters))
            .Returns(Task.CompletedTask);

        //act
        var updateResult = await _service.UpdateAsync(eventUpdateParameters);
        
        //assert
        Assert.NotNull(updateResult);
        Assert.True(updateResult.IsSuccess);
    }
}
