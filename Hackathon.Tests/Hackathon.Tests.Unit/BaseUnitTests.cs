﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Team;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Hackathon.DAL.Repositories;
using Hackathon.Tests.Common;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hackathon.Tests.Unit
{
    public abstract class BaseUnitTests: IDisposable
    {
        protected readonly ApplicationDbContext DbContext;

        protected readonly IUserRepository UserRepository;
        protected readonly IEventRepository EventRepository;
        protected readonly ITeamRepository TeamRepository;
        protected readonly IProjectRepository ProjectRepository;

        protected BaseUnitTests()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .AddJsonFile($"appsettings.Tests.Unit.json", true, false);

            var configurationRoot = configurationBuilder.Build();
            var connectionString = configurationRoot.GetConnectionString("DefaultConnectionString");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString)
                .LogTo(Console.WriteLine)
                .Options;

            DbContext = new ApplicationDbContext(options);
            DbContext.Database.EnsureCreated();

            var mapperConfig = new TypeAdapterConfig();
            mapperConfig.Apply(new IRegister[]
            {
                new EventEntityMapping(),
                new TeamEntityMapping(),
                new UserEntityMapping()
            });

            IMapper mapper = new Mapper(mapperConfig);

            UserRepository = new UserRepository(mapper, DbContext);
            EventRepository = new EventRepository(mapper, DbContext);
            TeamRepository = new TeamRepository(mapper, DbContext);
            ProjectRepository = new ProjectRepository(mapper, DbContext);
        }

        protected async Task<TeamModel> CreateTeamWithEvent()
        {
            var eventModel = TestFaker.GetCreateEventModels(1).First();
            var eventId = await EventRepository.CreateAsync(eventModel);

            var createTeamModel = TestFaker.GetCreateTeamModels(1).First();
            createTeamModel.EventId = eventId;

            var teamModel = createTeamModel.Adapt<TeamModel>();

            teamModel.Id = await TeamRepository.CreateAsync(createTeamModel);
            return teamModel;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}