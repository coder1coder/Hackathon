using System;
using System.Collections.Generic;
using Bogus;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Hackathon.Entities;
using Hackathon.Entities.User;
using MapsterMapper;

namespace Hackathon.Tests.Integration;

public class TestFaker
{
    private readonly IMapper _mapper;

    public TestFaker(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region Models

    public static IEnumerable<SignUpModel> GetSignUpModels(int count)
    {
        var faker = new Faker<SignUpModel>();

        faker
            .RuleFor(x => x.UserName, f => f.Person.UserName)
            .RuleFor(x => x.Password, f => f.Random.String2(6, 20))
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email);

        return faker.Generate(count);
    }

    public IEnumerable<EventModel> GetEventModels(int count, long userId, EventStatus? eventStatus = null)
    {
        var eventEntities = GetEventEntities(count, userId, eventStatus);
        return _mapper.Map<List<EventModel>>(eventEntities);
    }

    public IEnumerable<EventCreateParameters> GetCreateEventModels(int count, long userId)
    {
        var eventEntities = GetEventEntities(count, userId);
        return _mapper.Map<List<EventCreateParameters>>(eventEntities);
    }

    public IEnumerable<EventEntity> GetEventsEntities(int count, long userId, EventStatus? eventStatus = null)
    {
        return GetEventEntities(count, userId, eventStatus);
    }

    public static IEnumerable<CreateTeamModel> GetCreateTeamModels(int count)
    {
        var faker = new Faker<CreateTeamModel>();

        faker
            .RuleFor(x => x.Name, f => f.Random.String2(6, 20));

        return faker.Generate(count);
    }

    public static IEnumerable<ProjectCreateModel> GetProjectCreateModel(int count)
    {
        var faker = new Faker<ProjectCreateModel>();

        faker
            .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
            .RuleFor(x=>x.Description, f=>f.Random.String2(6,20));

        return faker.Generate(count);
    }

    #endregion

    #region Entities

    private static IEnumerable<EventEntity> GetEventEntities(int count, long ownerId, EventStatus? eventStatus = null)
    {
        var faker = new Faker<EventEntity>();

        faker
            .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
            .RuleFor(x => x.Start, DateTime.UtcNow.AddDays(1))
            .RuleFor(x => x.MemberRegistrationMinutes, f => f.Random.Int(1, 30))
            .RuleFor(x => x.DevelopmentMinutes, f => f.Random.Int(1, 30))
            .RuleFor(x => x.TeamPresentationMinutes, f => f.Random.Int(1, 30))
            .RuleFor(x => x.MaxEventMembers, _ => 30)
            .RuleFor(x => x.MinTeamMembers, _ => 3)
            .RuleFor(x => x.Status, _ => eventStatus ?? EventStatus.Draft)
            .RuleFor(x => x.OwnerId, ownerId)
            .RuleFor(x => x.ChangeEventStatusMessages, _ => new List<ChangeEventStatusMessage>())
            .RuleFor(x => x.Award, f => f.Random.Number(1, 1000).ToString())
            .RuleFor(x => x.Description, f => f.Random.String2(400));

        return faker.Generate(count);
    }

    public static IEnumerable<UserEntity> GetUserEntities(int count)
    {
        var faker = new Faker<UserEntity>();

        faker
            .RuleFor(x => x.UserName, f => f.Person.UserName)
            .RuleFor(x => x.PasswordHash, f => f.Random.String2(6, 20))
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email);

        return faker.Generate(count);
    }

    public static IEnumerable<TeamEntity> GetTeamEntities(int count, UserEntity owner)
    {
        var faker = new Faker<TeamEntity>();

        faker
            .RuleFor(x => x.Name, f => f.Company.CompanyName())
            .RuleFor(x => x.Events, _ => new List<EventEntity>())
            .RuleFor(x => x.Members, _ => new List<MemberTeamEntity>())
            .RuleFor(x => x.Owner, _ => owner)
            .RuleFor(x => x.OwnerId, _ => owner.Id);

        return faker.Generate(count);
    }

    #endregion

}
