﻿using System;
using System.Collections.Generic;
using Bogus;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Hackathon.DAL.Entities;
using MapsterMapper;

namespace Hackathon.Tests.Integration
{
    public class TestFaker
    {
        private readonly IMapper _mapper;
        public TestFaker(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Models

        public IEnumerable<SignUpModel> GetSignUpModels(int count)
        {
            var faker = new Faker<SignUpModel>();

            faker
                .RuleFor(x => x.UserName, f => f.Person.UserName)
                .RuleFor(x => x.Password, f => f.Random.String2(6, 20))
                .RuleFor(x => x.FullName, f => f.Person.FullName)
                .RuleFor(x => x.Email, f => f.Person.Email);

            return faker.Generate(count);
        }

        public IEnumerable<EventModel> GetEventModels(int count, EventStatus? eventStatus = null)
        {
            var eventEntities = GetEventEntities(count, eventStatus);
            return _mapper.Map<List<EventModel>>(eventEntities);
        }

        public IEnumerable<CreateEventModel> GetCreateEventModels(int count)
        {
            var eventEntities = GetEventEntities(count);
            return _mapper.Map<List<CreateEventModel>>(eventEntities);
        }

        public IEnumerable<CreateTeamModel> GetCreateTeamModels(int count)
        {
            var faker = new Faker<CreateTeamModel>();

            faker
                .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
                ;

            return faker.Generate(count);
        }

        public IEnumerable<ProjectCreateModel> GetProjectCreateModel(int count)
        {
            var faker = new Faker<ProjectCreateModel>();

            faker
                .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
                .RuleFor(x=>x.Description, f=>f.Random.String2(6,20))
                ;

            return faker.Generate(count);
        }

        #endregion

        #region Entities

        public IEnumerable<EventEntity> GetEventEntities(int count, EventStatus? eventStatus = null)
        {
            var faker = new Faker<EventEntity>();

            faker
                .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
                .RuleFor(x => x.Start, DateTime.UtcNow.AddDays(1))
                .RuleFor(x => x.MemberRegistrationMinutes, f=>f.Random.Int(1,30))
                .RuleFor(x => x.DevelopmentMinutes, f=>f.Random.Int(1,30))
                .RuleFor(x => x.TeamPresentationMinutes, f=>f.Random.Int(1,30))
                .RuleFor(x => x.MaxEventMembers, _=>30)
                .RuleFor(x => x.MinTeamMembers, _ => 3)
                .RuleFor(x => x.Status, _ => eventStatus ?? EventStatus.Draft)
                ;

            return faker.Generate(count);
        }

        public IEnumerable<UserEntity> GetUserEntities(int count)
        {
            var faker = new Faker<UserEntity>();

            faker
                .RuleFor(x => x.UserName, f => f.Person.UserName)
                .RuleFor(x => x.PasswordHash, f => f.Random.String2(6, 20))
                .RuleFor(x => x.FullName, f => f.Person.FullName)
                .RuleFor(x => x.Email, f => f.Person.Email);

            return faker.Generate(count);
        }

        #endregion

    }
}