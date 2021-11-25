using System;
using System.Collections.Generic;
using Bogus;
using Hackathon.Common.Entities;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Mapster;

namespace Hackathon.Tests.Common
{
    public static class TestFaker
    {
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

        public static IEnumerable<EventModel> GetEventModels(int count, EventStatus? eventStatus = null)
        {
            var faker = new Faker<EventModel>();

            faker
                .RuleFor(x => x.Name, f => f.Random.String2(6,20))
                .RuleFor(x => x.Status, f => eventStatus ?? f.PickRandom<EventStatus>())
                .RuleFor(x => x.Start, _ => DateTime.UtcNow.AddDays(1));

            return faker.Generate(count);
        }

        public static IEnumerable<CreateEventModel> GetCreateEventModels(int count)
        {
            return GetEventEntities(count).Adapt<List<CreateEventModel>>();
        }

        public static IEnumerable<CreateTeamModel> GetCreateTeamModels(int count)
        {
            var faker = new Faker<CreateTeamModel>();

            faker
                .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
                ;

            return faker.Generate(count);
        }

        #endregion

        #region Entities

        public static IEnumerable<EventEntity> GetEventEntities(int count, EventStatus? eventStatus = null)
        {
            var faker = new Faker<EventEntity>();

            faker
                .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
                .RuleFor(x => x.Start, DateTime.UtcNow.AddDays(1))
                .RuleFor(x => x.StartMemberRegistration, DateTime.UtcNow.AddDays(1).AddMinutes(30))
                .RuleFor(x => x.MaxEventMembers, _ => 30)
                .RuleFor(x => x.MinTeamMembers, _ => 3)
                .RuleFor(x => x.Status, _ => eventStatus ?? EventStatus.Draft)
                ;

            return faker.Generate(count);
        }

        #endregion

    }
}