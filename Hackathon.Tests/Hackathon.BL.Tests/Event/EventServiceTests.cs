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
using Hackathon.Common.Models.FileStorage;
using static System.Net.Mime.MediaTypeNames;
using Hackathon.Common.Models;
using Amazon.Runtime.Internal.Util;
using Hackathon.BL.User;
using Microsoft.Extensions.Logging;

namespace Hackathon.BL.Tests.Event;

public class EventServiceTests: BaseUnitTest
{
    private readonly EventService _service;
    private readonly Mock<IEventRepository> _eventRepositoryMock;
    private readonly Mock<IBus> _busMock;
    private readonly Mock<IFileStorageRepository> _fileStorageRepositoryMock;
    private readonly Mock<ILogger<EventService>> _loggerMock;

    public EventServiceTests()
    {
        var createValidatorMock = new Mock<IValidator<EventCreateParameters>>();
        var updateValidatorMock = new Mock<IValidator<EventUpdateParameters>>();
        var getListValidatorMock = new Mock<IValidator<GetListParameters<EventFilter>>>();
        var teamServiceMock = new Mock<ITeamService>();
        var notificationServiceMock = new Mock<INotificationService>();
        var fileStorageServiceMock = new Mock<IFileStorageService>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        var userRepositoryMock = new Mock<IUserRepository>();
        _busMock = new Mock<IBus>();
        var integrationEventHubMock = new Mock<IMessageHub<EventStatusChangedIntegrationEvent>>();
        var eventAgreementRepositoryMock = new Mock<IEventAgreementRepository>();
        _fileStorageRepositoryMock = new Mock<IFileStorageRepository>();
        _loggerMock = new Mock<ILogger<EventService>>();
        var eventImageValidator = new Mock<IValidator<IFileImage>>();

        _service = new EventService(
            createValidatorMock.Object,
            updateValidatorMock.Object,
            getListValidatorMock.Object,
            eventImageValidator.Object,
            _eventRepositoryMock.Object,
            teamServiceMock.Object,
            userRepositoryMock.Object,
            notificationServiceMock.Object,
            _busMock.Object,
            integrationEventHubMock.Object,
            fileStorageServiceMock.Object,
            _fileStorageRepositoryMock.Object,
            eventAgreementRepositoryMock.Object,
            _loggerMock.Object
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
    public async Task Create_HasImageId_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);
        var imageId = Guid.NewGuid();

        var eventCreateParameners = new EventCreateParameters
        {
            ImageId = imageId,
            Stages = new List<EventStageModel>()
        };

        _eventRepositoryMock.Setup(x => x.CreateAsync(eventCreateParameners))
            .ReturnsAsync(createdId);

        _fileStorageRepositoryMock.Setup(x => x.GetAsync(eventCreateParameners.ImageId.Value))
            .ReturnsAsync(new StorageFile());

        _fileStorageRepositoryMock.Setup(x => x.UpdateFlagIsDeleted(eventCreateParameners.ImageId.Value, false))
            .Returns(Task.CompletedTask);

        _busMock.Setup(x => x.Publish(It.IsAny<EventLogModel>(), default))
            .Returns(Task.CompletedTask);

        //act
        var result = await _service.CreateAsync(eventCreateParameners);

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

        _eventRepositoryMock.Setup(x => x.GetAsync(eventUpdateParameters.Id))
            .Returns(Task.FromResult(new EventModel()));

        _eventRepositoryMock.Setup(x => x.UpdateAsync(eventUpdateParameters))
            .Returns(Task.CompletedTask);

        //act
        var updateResult = await _service.UpdateAsync(eventUpdateParameters);

        //assert
        Assert.NotNull(updateResult);
        Assert.True(updateResult.IsSuccess);
    }

    [Fact]
    public async Task Update_Should_Return_Result_Success_With_Identical_File_Ids()
    {
        //arrange
        var eventUpdateParameters = new Faker<EventUpdateParameters>()
            .RuleFor(x => x.Id, f => f.Random.Long(1, 10))
            .RuleFor(x => x.ImageId, f => Guid.NewGuid())
            .Generate();

        _eventRepositoryMock.Setup(x => x.GetAsync(eventUpdateParameters.Id))
            .Returns(Task.FromResult(new EventModel()
            {
                ImageId = eventUpdateParameters.ImageId
            }));

        _eventRepositoryMock.Setup(x => x.UpdateAsync(eventUpdateParameters))
            .Returns(Task.CompletedTask);

        //act
        var updateResult = await _service.UpdateAsync(eventUpdateParameters);

        //assert
        Assert.NotNull(updateResult);
        Assert.True(updateResult.IsSuccess);
    }
}
