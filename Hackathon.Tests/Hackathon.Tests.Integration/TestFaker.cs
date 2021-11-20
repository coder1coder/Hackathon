using System;
using System.Collections.Generic;
using Bogus;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;

namespace Hackathon.Tests.Integration
{
    public static class TestFaker
    {
        public static List<SignUpModel> GetSignUpModels(int count)
        {
            var faker = new Faker<SignUpModel>();

            faker
                .RuleFor(x => x.UserName, f => f.Person.UserName)
                .RuleFor(x => x.Password, f => f.Random.String2(6, 20))
                .RuleFor(x => x.FullName, f => f.Person.FullName)
                .RuleFor(x => x.Email, f => f.Person.Email);

            return faker.Generate(count);
        }

        public static List<EventModel> GetEventModels(int count, EventStatus? eventStatus = null)
        {
            var faker = new Faker<EventModel>();

            faker
                .RuleFor(x => x.Name, f => f.Random.String2(6,20))
                .RuleFor(x => x.Status, f => eventStatus ?? f.PickRandom<EventStatus>())
                .RuleFor(x => x.Start, _ => DateTime.UtcNow.AddDays(1));

            return faker.Generate(count);
        }
    }
}