using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Notification;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.BL.Event;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Event;

public class EventServiceTests: BaseUnitTest
{
    private Mock<IValidator<CreateEventModel>> _createValidatorMock = new();
    private Mock<IValidator<UpdateEventModel>> _updateValidatorMock = new();
    private Mock<IValidator<GetListModel<EventFilterModel>>> _getListValidatorMock = new();
    
    private Mock<ITeamService> _teamServiceMock = new();
    private Mock<INotificationService> _notificationServiceMock = new ();
    
    private Mock<IEventRepository> _eventRepositoryMock = new();
    private Mock<IUserRepository> _userRepositoryMock = new();

    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);
        
        _createValidatorMock = new Mock<IValidator<CreateEventModel>>();
        _updateValidatorMock = new Mock<IValidator<UpdateEventModel>>();
        _getListValidatorMock = new Mock<IValidator<GetListModel<EventFilterModel>>>();
        
        _teamServiceMock = new Mock<ITeamService>();
        _notificationServiceMock = new Mock<INotificationService>();
        
        _eventRepositoryMock = new Mock<IEventRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _eventRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<CreateEventModel>()))
            .ReturnsAsync(createdId);
        
        var service = new EventService(
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _getListValidatorMock.Object,
            _eventRepositoryMock.Object,
            _teamServiceMock.Object,
            _userRepositoryMock.Object,
            _notificationServiceMock.Object
        );

        //act
        var result = await service.CreateAsync(new CreateEventModel());

        //assert
        result.Should().Be(createdId);
    }
}