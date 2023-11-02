using System;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Bogus;
using FluentValidation;
using Hackathon.BL.ApprovalApplications;
using Hackathon.Common.Abstraction.ApprovalApplications;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;
using Hackathon.Informing.Abstractions.Services;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.ApprovalApplication;

public class ApprovalApplicationTests: BaseUnitTest
{
    private readonly ApprovalApplicationService _approvalApplicationService;
    private readonly Mock<IApprovalApplicationRepository> _approvalApplicationRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    
    private const string NoRightsExecutingOperation = "Нет прав на выполнение операции";
    private const string ApprovalApplicationDoesntExist = "Заявка на согласование не найдена";
    private const string ApprovalApplicationAlreadyHasDecision = "По заявке на согласование уже вынесено решение";

    public ApprovalApplicationTests()
    {
        var validator = new Mock<IValidator<ApprovalApplicationRejectParameters>>();
        var notificationServiceMock = new Mock<INotificationService>();
        _approvalApplicationRepositoryMock = new Mock<IApprovalApplicationRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        
        _approvalApplicationService = new ApprovalApplicationService(
            _approvalApplicationRepositoryMock.Object,
            _userRepositoryMock.Object,
            validator.Object,
            notificationServiceMock.Object
        );
    }

    [Fact]
    public async Task GetListAsync_Should_Return_Array()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Role, UserRole.Administrator)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        var fakeRequests = new Faker<ApprovalApplicationModel>().Generate(4).ToArray();

        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        _approvalApplicationRepositoryMock
            .Setup(x => x.GetListAsync(It.IsAny<GetListParameters<ApprovalApplicationFilter>>()))
            .ReturnsAsync(new Page<ApprovalApplicationModel>
            {
                Items = fakeRequests,
                Total = fakeRequests.Length
            });
        
        //act
        var result = await _approvalApplicationService.GetListAsync(authorizedUserId, 
            new GetListParameters<ApprovalApplicationFilter>());
        
        //assert
        Assert.NotNull(result.Data);
        Assert.True(result.IsSuccess);
        Assert.Equal(fakeRequests.Length, result.Data.Total);
    }
    
    [Fact]
    public async Task GetListAsync_Should_Return_Forbidden()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Role, UserRole.Default)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        
        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        //act
        var result = await _approvalApplicationService.GetListAsync(authorizedUserId, 
            new GetListParameters<ApprovalApplicationFilter>());
        
        //assert
        Assert.Null(result.Data);
        Assert.False(result.IsSuccess);
        Assert.Equal(NoRightsExecutingOperation, result.Errors.Values["forbidden"]);
    }

    [Fact]
    public async Task GetAsync_Should_Return_ApprovalApplicationModel()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var approvalApplicationId = Random.Shared.NextInt64(1, long.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Id, authorizedUserId)
            .RuleFor(x => x.Role, UserRole.Administrator)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        var fakeApprovalApplicationModel = new Faker<ApprovalApplicationModel>()
            .RuleFor(x => x.Id, approvalApplicationId)
            .RuleFor(x => x.AuthorId, authorizedUserId);
        
        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        _approvalApplicationRepositoryMock
            .Setup(x => x.GetAsync(approvalApplicationId))
            .ReturnsAsync(fakeApprovalApplicationModel);
        
        //act
        var result = await _approvalApplicationService.GetAsync(authorizedUserId, approvalApplicationId);

        //assert
        Assert.NotNull(result.Data);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Data.Id, approvalApplicationId);
    }
    
    [Fact]
    public async Task GetAsync_Should_Return_NotFound()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var approvalApplicationId = Random.Shared.NextInt64(1, long.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Id, authorizedUserId)
            .RuleFor(x => x.Role, UserRole.Default)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        
        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        //act
        var result = await _approvalApplicationService.GetAsync(authorizedUserId, approvalApplicationId);

        //assert
        Assert.Null(result.Data);
        Assert.False(result.IsSuccess);
        Assert.Equal(ApprovalApplicationDoesntExist, result.Errors.Values["not-found"]);
    }
    
    [Fact]
    public async Task GetAsync_Should_Return_Forbidden_By_Owner()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var approvalApplicationId = Random.Shared.NextInt64(1, long.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Id, authorizedUserId)
            .RuleFor(x => x.Role, UserRole.Administrator)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        var fakeApprovalApplicationModel = new Faker<ApprovalApplicationModel>()
            .RuleFor(x => x.Id, approvalApplicationId);

        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        _approvalApplicationRepositoryMock
            .Setup(x => x.GetAsync(approvalApplicationId))
            .ReturnsAsync(fakeApprovalApplicationModel);
        
        //act
        var result = await _approvalApplicationService.GetAsync(authorizedUserId, approvalApplicationId);
        
        //assert
        Assert.Null(result.Data);
        Assert.False(result.IsSuccess);
        Assert.Equal(NoRightsExecutingOperation, result.Errors.Values["forbidden"]);
    }
    
    [Fact]
    public async Task ApproveAsync_Should_Return_Success()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var approvalApplicationId = Random.Shared.NextInt64(1, long.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Id, authorizedUserId)
            .RuleFor(x => x.Role, UserRole.Administrator)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        var fakeApprovalApplicationModel = new Faker<ApprovalApplicationModel>()
            .RuleFor(x => x.Id, approvalApplicationId)
            .RuleFor(x => x.AuthorId, authorizedUserId)
            .RuleFor(x => x.ApplicationStatus, ApprovalApplicationStatus.Requested);
        
        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        _approvalApplicationRepositoryMock
            .Setup(x => x.GetAsync(approvalApplicationId))
            .ReturnsAsync(fakeApprovalApplicationModel);
        
        //act
        var result = await _approvalApplicationService.ApproveAsync(authorizedUserId, approvalApplicationId);

        //assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task ApproveAsync_Should_Return_NotValid()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var approvalApplicationId = Random.Shared.NextInt64(1, long.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Id, authorizedUserId)
            .RuleFor(x => x.Role, UserRole.Administrator)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        var fakeApprovalApplicationModel = new Faker<ApprovalApplicationModel>()
            .RuleFor(x => x.Id, approvalApplicationId)
            .RuleFor(x => x.AuthorId, authorizedUserId)
            .RuleFor(x => x.ApplicationStatus, ApprovalApplicationStatus.Approved);
        
        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        _approvalApplicationRepositoryMock
            .Setup(x => x.GetAsync(approvalApplicationId))
            .ReturnsAsync(fakeApprovalApplicationModel);
        
        //act
        var result = await _approvalApplicationService.ApproveAsync(authorizedUserId, approvalApplicationId);
        
        //assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApprovalApplicationAlreadyHasDecision, result.Errors.Values["validation-error"]);
    }
    
    [Fact]
    public async Task RejectAsync_Should_Return_Success()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var approvalApplicationId = Random.Shared.NextInt64(1, long.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Id, authorizedUserId)
            .RuleFor(x => x.Role, UserRole.Administrator)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        var rejectParameters = new Faker<ApprovalApplicationRejectParameters>()
            .RuleFor(x => x.Comment, f => f.Random.String(10, 50))
            .Generate();
        var fakeApprovalApplicationModel = new Faker<ApprovalApplicationModel>()
            .RuleFor(x => x.Id, approvalApplicationId)
            .RuleFor(x => x.AuthorId, authorizedUserId)
            .RuleFor(x => x.ApplicationStatus, ApprovalApplicationStatus.Requested);
        
        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        _approvalApplicationRepositoryMock
            .Setup(x => x.GetAsync(approvalApplicationId))
            .ReturnsAsync(fakeApprovalApplicationModel);
        
        //act
        var result = await _approvalApplicationService.RejectAsync(authorizedUserId, approvalApplicationId, rejectParameters);

        //assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task RejectAsync_Should_Return_NotValid()
    {
        //arrange
        var authorizedUserId = Random.Shared.Next(1, int.MaxValue);
        var approvalApplicationId = Random.Shared.NextInt64(1, long.MaxValue);
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.Id, authorizedUserId)
            .RuleFor(x => x.Role, UserRole.Administrator)
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();
        var rejectParameters = new Faker<ApprovalApplicationRejectParameters>()
            .RuleFor(x => x.Comment, f => f.Random.String(10, 50))
            .Generate();
        
        var fakeApprovalApplicationModel = new Faker<ApprovalApplicationModel>()
            .RuleFor(x => x.Id, approvalApplicationId)
            .RuleFor(x => x.AuthorId, authorizedUserId)
            .RuleFor(x => x.ApplicationStatus, ApprovalApplicationStatus.Approved);
        
        _userRepositoryMock
            .Setup(x => x.GetAsync(authorizedUserId))
            .ReturnsAsync(fakeUser);
        
        _approvalApplicationRepositoryMock
            .Setup(x => x.GetAsync(approvalApplicationId))
            .ReturnsAsync(fakeApprovalApplicationModel);
        
        //act
        var result = await _approvalApplicationService.RejectAsync(authorizedUserId, approvalApplicationId, rejectParameters);
        
        //assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApprovalApplicationAlreadyHasDecision, result.Errors.Values["validation-error"]);
    }
}
