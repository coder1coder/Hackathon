using System;
using Hackathon.Abstraction;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Project;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.DAL;
using Hackathon.DAL.Repositories;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hackathon.Tests.Integration.Base
{
    public abstract class BaseRepositoryTests: IDisposable
    {
        protected readonly ApplicationDbContext DbContext;

        protected readonly IUserRepository UserRepository;
        protected readonly IEventRepository EventRepository;
        protected readonly ITeamRepository TeamRepository;
        protected readonly IProjectRepository ProjectRepository;

        protected BaseRepositoryTests()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .AddJsonFile($"appsettings.Tests.json", true, false);

            var configurationRoot = configurationBuilder.Build();
            var connectionString = configurationRoot.GetConnectionString("DefaultConnectionString");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString)
                .LogTo(Console.WriteLine)
                .Options;

            DbContext = new ApplicationDbContext(options);
            DbContext.Database.EnsureCreated();

            IMapper mapper = new Mapper();

            UserRepository = new UserRepository(mapper, DbContext);
            EventRepository = new EventRepository(mapper, DbContext);
            TeamRepository = new TeamRepository(mapper, DbContext);
            ProjectRepository = new ProjectRepository(mapper, DbContext);
        }



        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}